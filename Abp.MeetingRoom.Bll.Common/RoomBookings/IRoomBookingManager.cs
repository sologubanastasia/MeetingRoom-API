using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
namespace Abp.MeetingRoom.Bll.Common.RoomBookings;
public interface IRoomBookingManager
{
    Task<RoomBooking> CreateRoomBookingAsync(RoomBooking booking);
    Task<IReadOnlyList<RoomBooking>> GetAllRoomBookingsAsync();
    Task<RoomBooking?> GetRoomBookingByIdAsync(Guid id);
    Task<bool> CancelRoomBookingAsync(Guid id);
    Task<IReadOnlyList<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<RoomBooking>> GetActiveByPeriodAsync(DateTime from, DateTime to);
}
