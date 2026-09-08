using System.Net.Http.Json;
using WebApi.LoadTesting.Models;

namespace WebApi.LoadTesting.Requests;

internal sealed class LoadTestRequestFactory
{
    public (LoadTestOperation Operation, HttpRequestMessage Request) Create(
        int requestIndex,
        Guid? roomId
    )
    {
        var operation = SelectOperation(requestIndex);
        if (operation == LoadTestOperation.UpdateRoom && !roomId.HasValue)
        {
            operation = LoadTestOperation.GetRooms;
        }

        return (operation, BuildRequest(operation, requestIndex, roomId));
    }

    private static LoadTestOperation SelectOperation(int requestIndex) =>
        (requestIndex % 10) switch
        {
            <= 3 => LoadTestOperation.GetRooms,
            <= 5 => LoadTestOperation.GetAvailable,
            6 => LoadTestOperation.GetBookings,
            <= 8 => LoadTestOperation.CreateRoom,
            _ => LoadTestOperation.UpdateRoom,
        };

    private static HttpRequestMessage BuildRequest(
        LoadTestOperation operation,
        int requestIndex,
        Guid? roomId
    )
    {
        var startTime = DateTime.UtcNow.AddDays(30);

        return operation switch
        {
            LoadTestOperation.GetRooms => new(HttpMethod.Get, "api/rooms"),
            LoadTestOperation.GetAvailable => new(
                HttpMethod.Get,
                BuildAvailabilityUrl(startTime)
            ),
            LoadTestOperation.GetBookings => new(HttpMethod.Get, "api/room-bookings"),
            LoadTestOperation.CreateRoom => CreateJsonRequest(
                HttpMethod.Post,
                "api/rooms",
                CreateRoomBody(requestIndex, updated: false)
            ),
            LoadTestOperation.UpdateRoom => CreateJsonRequest(
                HttpMethod.Put,
                $"api/rooms/{roomId!.Value}",
                CreateRoomBody(requestIndex, updated: true)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
    }

    private static string BuildAvailabilityUrl(DateTime startTime) =>
        $"api/rooms/available?startTime={Uri.EscapeDataString(startTime.ToString("O"))}" +
        $"&endTime={Uri.EscapeDataString(startTime.AddHours(2).ToString("O"))}&capacity=2";

    private static HttpRequestMessage CreateJsonRequest(
        HttpMethod method,
        string url,
        object body
    ) => new(method, url) { Content = JsonContent.Create(body) };

    private static object CreateRoomBody(int requestIndex, bool updated) => new
    {
        name = $"Load test {(updated ? "updated" : "room")} " +
            $"{Environment.ProcessId}-{requestIndex}",
        capacity = 2 + requestIndex % 20,
        pricePerHour = 100 + requestIndex % 50,
        options = Array.Empty<object>(),
    };
}
