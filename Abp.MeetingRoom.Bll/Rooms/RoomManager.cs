using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Bll.Common.Rooms.Models;
namespace Abp.MeetingRoom.Bll.Rooms;
public sealed class RoomManager : IRoomManager
{
    private readonly IRoomRepository _roomRepository;
    public RoomManager(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    public async Task<IReadOnlyList<Room>> GetAllRoomsAsync(CancellationToken cancellationToken)
    {
        return await _roomRepository.GetAllAsync(cancellationToken);
    }
    public async Task<Room?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _roomRepository.GetByIdAsync(id, cancellationToken);
    }
    public async Task<Room> CreateRoomAsync(Room room, CancellationToken cancellationToken)
    {
        foreach (var option in room.Options)
        {
            option.IsActive = true;
        }
        return await _roomRepository.CreateAsync(room, cancellationToken);
    }
    public async Task<Room?> UpdateRoomAsync(Room room, CancellationToken cancellationToken)
    {
        var existingRoom = await _roomRepository.GetByIdAsync(room.Id, cancellationToken);
        if (existingRoom is null)
        {
            return null;
        }
        existingRoom.Name = room.Name;
        existingRoom.Capacity = room.Capacity;
        existingRoom.PricePerHour = room.PricePerHour;
        existingRoom.Options = room.Options
            .Select(option => new RoomOption
            {
                RoomId = existingRoom.Id,
                Name = option.Name,
                Price = option.Price,
                IsActive = true,
            })
            .ToList();
        return await _roomRepository.UpdateAsync(existingRoom, cancellationToken);
    }
    public async Task<bool> DeleteRoomAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _roomRepository.SoftDeleteAsync(id, cancellationToken);
    }
    public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startTime,
        DateTime endTime,
        int capacity,
        CancellationToken cancellationToken
    )
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be earlier than end time.");
        }
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.");
        }
        return await _roomRepository.GetAvailableAsync(
            startTime,
            endTime,
            capacity,
            cancellationToken
        );
    }
}
