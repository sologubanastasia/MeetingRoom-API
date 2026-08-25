using Abp.MeetingRoom.Bll.Common.Rooms.Models;
namespace Abp.MeetingRoom.Bll.Common.Rooms;
public interface IRoomManager
{
    Task<IReadOnlyList<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomByIdAsync(Guid id);
    Task<Room> CreateRoomAsync(Room room);
    Task<Room?> UpdateRoomAsync(Room room);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startTime,
        DateTime endTime,
        int capacity
    );
}
