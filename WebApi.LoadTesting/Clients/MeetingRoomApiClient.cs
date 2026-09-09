using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.LoadTesting.Models;
using WebApi.LoadTesting.Requests;

namespace WebApi.LoadTesting.Clients;

internal sealed class MeetingRoomApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;
    private readonly LoadTestRequestFactory _requestFactory;
    private readonly ConcurrentBag<Guid> _roomIds = new();

    public MeetingRoomApiClient(HttpClient httpClient, LoadTestRequestFactory requestFactory)
    {
        _httpClient = httpClient;
        _requestFactory = requestFactory;
    }

    public async Task InitializeAsync()
    {
        var rooms = await _httpClient.GetFromJsonAsync<List<RoomIdentifierResponse>>(
            "api/rooms",
            JsonOptions
        ) ?? throw new JsonException("The rooms endpoint returned an empty response body.");

        foreach (var room in rooms)
        {
            _roomIds.Add(room.Id);
        }
    }

    public async Task<RequestResult> SendAsync(int requestIndex)
    {
        var roomId = _roomIds.TryPeek(out var id) ? id : (Guid?)null;
        var (operation, request) = _requestFactory.Create(requestIndex, roomId);
        using (request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                using var response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead
                );

                if (operation == LoadTestOperation.CreateRoom && response.IsSuccessStatusCode)
                {
                    await RememberCreatedRoomAsync(response);
                }

                return new RequestResult(
                    operation,
                    stopwatch.Elapsed.TotalMilliseconds,
                    response.IsSuccessStatusCode,
                    ((int)response.StatusCode).ToString()
                );
            }
            catch (Exception exception) when (
                exception is HttpRequestException or TaskCanceledException or JsonException
            )
            {
                return new RequestResult(
                    operation,
                    stopwatch.Elapsed.TotalMilliseconds,
                    false,
                    exception.Message
                );
            }
        }
    }

    private async Task RememberCreatedRoomAsync(HttpResponseMessage response)
    {
        var room = await response.Content.ReadFromJsonAsync<RoomIdentifierResponse>(JsonOptions)
            ?? throw new JsonException("The create-room endpoint returned an empty response body.");

        _roomIds.Add(room.Id);
    }
}
