using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.LoadTesting.Configuration;
using WebApi.LoadTesting.Metrics;
using WebApi.LoadTesting.Models;
using WebApi.LoadTesting.Output;

var config = CommandLineOptionsParser.Parse(args);
var reportWriter = new ConsoleReportWriter();
using var handler = new SocketsHttpHandler { MaxConnectionsPerServer = config.Concurrency };
using var http = new HttpClient(handler)
{
    BaseAddress = new Uri(config.Url.TrimEnd('/') + "/"),
    Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds)
};

reportWriter.WriteHeader(config);

var roomIds = await LoadRoomIds(http);
var metrics = new MetricsCollector();
using var semaphore = new SemaphoreSlim(config.Concurrency);
var total = Stopwatch.StartNew();
var tasks = Enumerable.Range(0, config.Requests).Select(async index =>
{
    await semaphore.WaitAsync();
    try
    {
        metrics.Add(await Send(index, http, roomIds));
    }
    finally
    {
        semaphore.Release();
    }
}).ToArray();

await Task.WhenAll(tasks);
total.Stop();
reportWriter.WriteResults(metrics.CreateSnapshot(), total.Elapsed, config);
return metrics.Failed > 0 ? 2 : 0;

static async Task<RequestResult> Send(int index, HttpClient http, ConcurrentBag<Guid> roomIds)
{
    var operation = (index % 10) switch
    {
        <= 3 => LoadTestOperation.GetRooms,
        <= 5 => LoadTestOperation.GetAvailable,
        6 => LoadTestOperation.GetBookings,
        <= 8 => LoadTestOperation.CreateRoom,
        _ => LoadTestOperation.UpdateRoom
    };
    using var request = BuildRequest(operation, index, roomIds);
    var timer = Stopwatch.StartNew();
    try
    {
        using var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (operation == LoadTestOperation.CreateRoom && response.IsSuccessStatusCode)
            await RememberRoom(response, roomIds);
        timer.Stop();
        return new(operation, timer.Elapsed.TotalMilliseconds, response.IsSuccessStatusCode,
            ((int)response.StatusCode).ToString());
    }
    catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
    {
        timer.Stop();
        return new(operation, timer.Elapsed.TotalMilliseconds, false, ex.Message);
    }
}
static HttpRequestMessage BuildRequest(LoadTestOperation operation, int index, ConcurrentBag<Guid> ids)
{
    var start = DateTime.UtcNow.AddDays(30);
    return operation switch
    {
        LoadTestOperation.GetRooms => new(HttpMethod.Get, "api/rooms"),
        LoadTestOperation.GetAvailable => new(HttpMethod.Get,
            $"api/rooms/available?startTime={Uri.EscapeDataString(start.ToString("O"))}" +
            $"&endTime={Uri.EscapeDataString(start.AddHours(2).ToString("O"))}&capacity=2"),
        LoadTestOperation.GetBookings => new(HttpMethod.Get, "api/room-bookings"),
        LoadTestOperation.CreateRoom => WithJson(HttpMethod.Post, "api/rooms", RoomBody(index, false)),
        LoadTestOperation.UpdateRoom when ids.TryPeek(out var id) =>
            WithJson(HttpMethod.Put, $"api/rooms/{id}", RoomBody(index, true)),
        LoadTestOperation.UpdateRoom => new(HttpMethod.Get, "api/rooms"),
        _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };
}

static HttpRequestMessage WithJson(HttpMethod method, string url, object body) =>
    new(method, url) { Content = JsonContent.Create(body) };

static object RoomBody(int index, bool updated) => new
{
    name = $"Load test {(updated ? "updated" : "room")} {Environment.ProcessId}-{index}",
    capacity = 2 + index % 20,
    pricePerHour = 100 + index % 50,
    options = Array.Empty<object>()
};

static async Task RememberRoom(HttpResponseMessage response, ConcurrentBag<Guid> ids)
{
    try
    {
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);
        if (json.RootElement.TryGetProperty("id", out var value) && value.TryGetGuid(out var id))
            ids.Add(id);
    }
    catch (JsonException) { }
}

static async Task<ConcurrentBag<Guid>> LoadRoomIds(HttpClient http)
{
    var ids = new ConcurrentBag<Guid>();
    try
    {
        await using var stream = await http.GetStreamAsync("api/rooms");
        using var json = await JsonDocument.ParseAsync(stream);
        foreach (var room in json.RootElement.EnumerateArray())
            if (room.TryGetProperty("id", out var value) && value.TryGetGuid(out var id)) ids.Add(id);
    }
    catch (Exception ex) when (ex is HttpRequestException or JsonException)
    {
        Console.Error.WriteLine($"API is unavailable or returned invalid data: {ex.Message}");
        Environment.Exit(1);
    }
    return ids;
}
