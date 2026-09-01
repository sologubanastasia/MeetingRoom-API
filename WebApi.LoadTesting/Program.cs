using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

var config = Config.Parse(args);
using var handler = new SocketsHttpHandler { MaxConnectionsPerServer = config.Concurrency };
using var http = new HttpClient(handler)
{
    BaseAddress = new Uri(config.Url.TrimEnd('/') + "/"),
    Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds)
};

Console.WriteLine($"Load test: {config.Requests} tasks / {config.Concurrency} concurrent requests");
Console.WriteLine($"Target: {http.BaseAddress}\n");

var roomIds = await LoadRoomIds(http);
var metrics = new Metrics();
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
PrintResults(metrics, total.Elapsed, config);
return metrics.Failed > 0 ? 2 : 0;

static async Task<Result> Send(int index, HttpClient http, ConcurrentBag<Guid> roomIds)
{
    var operation = (index % 10) switch
    {
        <= 3 => Operation.GetRooms,
        <= 5 => Operation.GetAvailable,
        6 => Operation.GetBookings,
        <= 8 => Operation.CreateRoom,
        _ => Operation.UpdateRoom
    };
    using var request = BuildRequest(operation, index, roomIds);
    var timer = Stopwatch.StartNew();
    try
    {
        using var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (operation == Operation.CreateRoom && response.IsSuccessStatusCode)
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

static HttpRequestMessage BuildRequest(Operation operation, int index, ConcurrentBag<Guid> ids)
{
    var start = DateTime.UtcNow.AddDays(30);
    return operation switch
    {
        Operation.GetRooms => new(HttpMethod.Get, "api/rooms"),
        Operation.GetAvailable => new(HttpMethod.Get,
            $"api/rooms/available?startTime={Uri.EscapeDataString(start.ToString("O"))}" +
            $"&endTime={Uri.EscapeDataString(start.AddHours(2).ToString("O"))}&capacity=2"),
        Operation.GetBookings => new(HttpMethod.Get, "api/room-bookings"),
        Operation.CreateRoom => WithJson(HttpMethod.Post, "api/rooms", RoomBody(index, false)),
        Operation.UpdateRoom when ids.TryPeek(out var id) =>
            WithJson(HttpMethod.Put, $"api/rooms/{id}", RoomBody(index, true)),
        Operation.UpdateRoom => new(HttpMethod.Get, "api/rooms"),
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

static void PrintResults(Metrics metrics, TimeSpan elapsed, Config config)
{
    var snapshot = metrics.Snapshot();
    Console.WriteLine("Results");
    Console.WriteLine($"Total time:       {elapsed.TotalSeconds:F3} s");
    Console.WriteLine($"Average response: {snapshot.Average:F2} ms");
    Console.WriteLine($"Minimum response: {snapshot.Minimum:F2} ms");
    Console.WriteLine($"Maximum response: {snapshot.Maximum:F2} ms");
    Console.WriteLine($"Throughput:       {snapshot.Count / elapsed.TotalSeconds:F2} req/s");
    Console.WriteLine($"Successful:       {snapshot.Successful}");
    Console.WriteLine($"Failed:           {snapshot.Failed}\n");
    foreach (var item in snapshot.Operations.OrderBy(x => x.Key))
        Console.WriteLine($"{item.Key,-14} count={item.Value.Count,10} ok={item.Value.Successful,10} " +
                          $"avg={item.Value.Average,8:F2} ms");
    foreach (var error in snapshot.Errors.OrderByDescending(x => x.Value).Take(10))
        Console.WriteLine($"ERROR {error.Value,10} x {error.Key}");
    Console.WriteLine($"\nConfiguration: {config.Requests} tasks / {config.Concurrency} concurrent requests");
}

internal sealed record Config(string Url, int Requests, int Concurrency, int TimeoutSeconds)
{
    public static Config Parse(string[] args)
    {
        if (args.Contains("--help"))
        {
            Console.WriteLine("--url <url> --requests <number> --concurrency <number> --timeout <seconds>");
            Environment.Exit(0);
        }
        string Value(string key, string fallback)
        {
            var index = Array.IndexOf(args, key);
            return index >= 0 && index + 1 < args.Length ? args[index + 1] : fallback;
        }
        int Positive(string key, int fallback)
        {
            var raw = Value(key, fallback.ToString());
            if (!int.TryParse(raw, out var value) || value <= 0)
                throw new ArgumentException($"{key} must be a positive integer.");
            return value;
        }
        return new(Value("--url", "http://localhost:5119"), Positive("--requests", 1000),
            Positive("--concurrency", 10), Positive("--timeout", 30));
    }
}

internal sealed record Result(Operation Operation, double Milliseconds, bool Success, string Status);
internal enum Operation { GetRooms, GetAvailable, GetBookings, CreateRoom, UpdateRoom }

internal sealed class Metrics
{
    private readonly object _gate = new();
    private readonly Dictionary<Operation, OperationMetrics> _operations = new();
    private readonly Dictionary<string, long> _errors = new();
    private long _count;
    private long _successful;
    private double _totalMilliseconds;
    private double _minimum = double.MaxValue;
    private double _maximum;

    public long Failed { get { lock (_gate) return _count - _successful; } }

    public void Add(Result result)
    {
        lock (_gate)
        {
            _count++;
            if (result.Success) _successful++;
            _totalMilliseconds += result.Milliseconds;
            _minimum = Math.Min(_minimum, result.Milliseconds);
            _maximum = Math.Max(_maximum, result.Milliseconds);

            if (!_operations.TryGetValue(result.Operation, out var operation))
                _operations[result.Operation] = operation = new OperationMetrics();
            operation.Count++;
            if (result.Success) operation.Successful++;
            operation.TotalMilliseconds += result.Milliseconds;

            if (!result.Success)
                _errors[result.Status] = _errors.GetValueOrDefault(result.Status) + 1;
        }
    }

    public MetricsSnapshot Snapshot()
    {
        lock (_gate)
        {
            return new(_count, _successful, _count - _successful,
                _count == 0 ? 0 : _totalMilliseconds / _count,
                _count == 0 ? 0 : _minimum, _maximum,
                _operations.ToDictionary(x => x.Key,
                    x => new OperationSnapshot(x.Value.Count, x.Value.Successful,
                        x.Value.Count == 0 ? 0 : x.Value.TotalMilliseconds / x.Value.Count)),
                new Dictionary<string, long>(_errors));
        }
    }

    private sealed class OperationMetrics
    {
        public long Count;
        public long Successful;
        public double TotalMilliseconds;
    }
}

internal sealed record MetricsSnapshot(long Count, long Successful, long Failed,
    double Average, double Minimum, double Maximum,
    IReadOnlyDictionary<Operation, OperationSnapshot> Operations,
    IReadOnlyDictionary<string, long> Errors);
internal sealed record OperationSnapshot(long Count, long Successful, double Average);
