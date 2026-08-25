using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using Abp.MeetingRoom.Services.Web.RoomBookings.Dtos;
namespace Abp.MeetingRoom.Services.Web.RoomBookings.Mappings;
public static class RoomBookingMapper
{
    public static RoomBooking ToModel(CreateRoomBookingRequest request)
    {
        return new RoomBooking
        {
            RoomId = request.RoomId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            SelectedOptions = request.SelectedOptionIds
                .Select(id => new BookingOption { RoomOptionId = id })
                .ToList(),
        };
    }
    public static RoomBookingResponse ToResponse(RoomBooking booking)
    {
        return new RoomBookingResponse
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = booking.Room?.Name ?? string.Empty,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            RoomPrice = booking.RoomPrice,
            OptionsPrice = booking.OptionsPrice,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt,
            SelectedOptions = booking.SelectedOptions
                .Select(option => new BookingOptionResponse
                {
                    Id = option.Id,
                    RoomOptionId = option.RoomOptionId,
                    OptionName = option.OptionName,
                    OptionPrice = option.OptionPrice,
                })
                .ToList(),
        };
    }
    public static IReadOnlyList<RoomBookingResponse> ToResponses(
        IEnumerable<RoomBooking> bookings
    )
    {
        return bookings.Select(ToResponse).ToList();
    }
}
