using MeetingRoom.Application.Dtos.Rooms;

namespace MeetingRoom.Application.Services.Rooms
{
    /// <summary>
    /// Визначає операції для управління конференц-залами.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Повертає всі конференц-зали.
        /// </summary>
        Task<List<RoomResponse>> GetAllRoomsAsync();

        /// <summary>
        /// Повертає конференц-зал за його ідентифікатором.
        /// </summary>
        Task<RoomResponse?> GetRoomByIdAsync(Guid id);

        /// <summary>
        /// Створює новий конференц-зал.
        /// </summary>
        Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request);

        /// <summary>
        /// Оновлює інформацію про конференц-зал.
        /// </summary>
        Task<RoomResponse?> UpdateRoomAsync(Guid id, UpdateRoomRequest request);

        /// <summary>
        /// Видаляє конференц-зал.
        /// </summary>
        Task<bool> DeleteRoomAsync(Guid id);

        /// <summary>
        /// Повертає доступні зали за часом і місткістю.
        /// </summary>
        Task<List<RoomResponse>> GetAvailableRoomsAsync(
            DateTime startTime,
            DateTime endTime,
            int capacity
        );
    }
}
