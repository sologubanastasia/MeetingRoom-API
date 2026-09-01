using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Entities;
using Microsoft.Data.SqlClient;
namespace Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Mappings;
internal static class RoomBookingDataMapper
{
    public static async Task<List<RoomBooking>> ReadAsync(SqlDataReader reader)
    {
        var entities = new Dictionary<Guid, RoomBookingEntity>();
        while (await reader.ReadAsync())
        {
            var bookingId = reader.GetGuid(reader.GetOrdinal("Id"));
            if (!entities.TryGetValue(bookingId, out var booking))
            {
                booking = new RoomBookingEntity
                {
                    Id = bookingId,
                    RoomId = reader.GetGuid(reader.GetOrdinal("RoomId")),
                    RoomName = reader.GetString(reader.GetOrdinal("RoomName")),
                    StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                    EndTime = reader.GetDateTime(reader.GetOrdinal("EndTime")),
                    RoomPrice = reader.GetDecimal(reader.GetOrdinal("RoomPrice")),
                    OptionsPrice = reader.GetDecimal(reader.GetOrdinal("OptionsPrice")),
                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                    Status = (BookingStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                };
                entities.Add(bookingId, booking);
            }
            var optionIdOrdinal = reader.GetOrdinal("BookingOptionId");
            if (!reader.IsDBNull(optionIdOrdinal))
            {
                booking.SelectedOptions.Add(new BookingOptionEntity
                {
                    Id = reader.GetGuid(optionIdOrdinal),
                    RoomBookingId = bookingId,
                    RoomOptionId = reader.GetGuid(reader.GetOrdinal("RoomOptionId")),
                    OptionName = reader.GetString(reader.GetOrdinal("OptionName")),
                    OptionPrice = reader.GetDecimal(reader.GetOrdinal("OptionPrice")),
                });
            }
        }
        return entities.Values.Select(ToModel).ToList();
    }
    private static RoomBooking ToModel(RoomBookingEntity entity)
    {
        var booking = new RoomBooking
        {
            Id = entity.Id,
            RoomId = entity.RoomId,
            Room = new Room { Id = entity.RoomId, Name = entity.RoomName },
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            RoomPrice = entity.RoomPrice,
            OptionsPrice = entity.OptionsPrice,
            TotalPrice = entity.TotalPrice,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
        };
        booking.SelectedOptions = entity.SelectedOptions
            .Select(option => new BookingOption
            {
                Id = option.Id,
                RoomBookingId = option.RoomBookingId,
                RoomBooking = booking,
                RoomOptionId = option.RoomOptionId,
                OptionName = option.OptionName,
                OptionPrice = option.OptionPrice,
            })
            .ToList();
        return booking;
    }
}
