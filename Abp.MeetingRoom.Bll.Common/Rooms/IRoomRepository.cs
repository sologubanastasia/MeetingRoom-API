using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
namespace Abp.MeetingRoom.Bll.Common.Rooms;
public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(Guid id);
    Task<Room> CreateAsync(Room room);
    Task<Room> UpdateAsync(Room room);
    Task<bool> SoftDeleteAsync(Guid id);
    Task<List<Room>> GetAvailableAsync(DateTime startTime, DateTime endTime, int capacity);
}
