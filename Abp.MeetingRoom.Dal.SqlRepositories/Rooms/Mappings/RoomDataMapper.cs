using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Entities;
using Microsoft.Data.SqlClient;
namespace Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Mappings;
internal static class RoomDataMapper
{
    public static async Task<List<Room>> ReadAsync(SqlDataReader reader)
    {
        var entities = new Dictionary<Guid, RoomEntity>();
        while (await reader.ReadAsync())
        {
            var roomId = reader.GetGuid(reader.GetOrdinal("Id"));
            if (!entities.TryGetValue(roomId, out var room))
            {
                room = new RoomEntity
                {
                    Id = roomId,
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                    PricePerHour = reader.GetDecimal(reader.GetOrdinal("PricePerHour")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                };
                entities.Add(roomId, room);
            }
            var optionIdOrdinal = reader.GetOrdinal("OptionId");
            if (!reader.IsDBNull(optionIdOrdinal))
            {
                room.Options.Add(new RoomOptionEntity
                {
                    Id = reader.GetGuid(optionIdOrdinal),
                    RoomId = roomId,
                    Name = reader.GetString(reader.GetOrdinal("OptionName")),
                    Price = reader.GetDecimal(reader.GetOrdinal("OptionPrice")),
                });
            }
        }
        return entities.Values.Select(ToModel).ToList();
    }
    private static Room ToModel(RoomEntity entity)
    {
        var room = new Room
        {
            Id = entity.Id,
            Name = entity.Name,
            Capacity = entity.Capacity,
            PricePerHour = entity.PricePerHour,
            CreatedAt = entity.CreatedAt,
            IsDeleted = false,
        };
        room.Options = entity.Options
            .Select(option => new RoomOption
            {
                Id = option.Id,
                RoomId = option.RoomId,
                Room = room,
                Name = option.Name,
                Price = option.Price,
                IsActive = true,
            })
            .ToList();
        return room;
    }
}
