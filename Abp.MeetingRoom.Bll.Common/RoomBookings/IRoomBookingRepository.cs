using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
namespace Abp.MeetingRoom.Bll.Common.RoomBookings;
public interface IRoomBookingRepository
{
    Task<List<RoomBooking>> GetAllAsync();
    Task<RoomBooking?> GetByIdAsync(Guid id);
    Task<RoomBooking> CreateAsync(
        Guid roomId,
        DateTime startTime,
        DateTime endTime,
        IReadOnlyCollection<Guid> selectedOptionIds
    );
    Task<bool> CancelAsync(Guid id);
    Task<List<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to);
    Task<List<RoomBooking>> GetActiveByPeriodAsync(DateTime from, DateTime to);
}
