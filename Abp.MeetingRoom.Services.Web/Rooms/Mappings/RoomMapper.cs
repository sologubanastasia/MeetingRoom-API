using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Services.Web.Rooms.Dtos;
namespace Abp.MeetingRoom.Services.Web.Rooms.Mappings;
public static class RoomMapper
{
    public static Room ToModel(CreateRoomRequest request)
    {
        return new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            PricePerHour = request.PricePerHour,
            Options = request.Options
                .Select(option => new RoomOption
                {
                    Name = option.Name,
                    Price = option.Price,
                    IsActive = true,
                })
                .ToList(),
        };
    }
    public static Room ToModel(Guid id, UpdateRoomRequest request)
    {
        return new Room
        {
            Id = id,
            Name = request.Name,
            Capacity = request.Capacity,
            PricePerHour = request.PricePerHour,
            Options = request.Options
                .Select(option => new RoomOption
                {
                    RoomId = id,
                    Name = option.Name,
                    Price = option.Price,
                    IsActive = true,
                })
                .ToList(),
        };
    }
    public static RoomResponse ToResponse(Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            PricePerHour = room.PricePerHour,
            Options = room.Options
                .Where(option => option.IsActive)
                .Select(option => new RoomOptionResponse
                {
                    Id = option.Id,
                    Name = option.Name,
                    Price = option.Price,
                })
                .ToList(),
        };
    }
    public static IReadOnlyList<RoomResponse> ToResponses(IEnumerable<Room> rooms)
    {
        return rooms.Select(ToResponse).ToList();
    }
}
