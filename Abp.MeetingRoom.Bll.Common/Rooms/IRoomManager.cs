using Abp.MeetingRoom.Bll.Common.Rooms.Models;

namespace Abp.MeetingRoom.Bll.Common.Rooms;

/// <summary>
/// Визначає бізнес-операції для керування конференц-залами.
/// </summary>
public interface IRoomManager
{
    /// <summary>
    /// Отримує всі активні конференц-зали.
    /// </summary>
    /// <returns>Колекція активних конференц-залів.</returns>
    Task<IReadOnlyList<Room>> GetAllRoomsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Отримує конференц-зал за його ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <returns>Знайдений зал або <see langword="null" />, якщо зал не існує.</returns>
    Task<Room?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Створює новий конференц-зал.
    /// </summary>
    /// <param name="room">Дані нового конференц-залу.</param>
    /// <returns>Створений конференц-зал.</returns>
    Task<Room> CreateRoomAsync(Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Оновлює наявний конференц-зал.
    /// </summary>
    /// <param name="room">Актуальні дані конференц-залу.</param>
    /// <returns>Оновлений зал або <see langword="null" />, якщо зал не існує.</returns>
    Task<Room?> UpdateRoomAsync(Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Виконує логічне видалення конференц-залу.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <returns><see langword="true" />, якщо зал видалено; інакше — <see langword="false" />.</returns>
    Task<bool> DeleteRoomAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Знаходить зали, доступні для бронювання у вказаний період.
    /// </summary>
    /// <param name="startTime">Дата й час початку періоду в UTC.</param>
    /// <param name="endTime">Дата й час завершення періоду в UTC.</param>
    /// <param name="capacity">Мінімальна необхідна місткість залу.</param>
    /// <returns>Колекція доступних конференц-залів.</returns>
    Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startTime,
        DateTime endTime,
        int capacity,
        CancellationToken cancellationToken
    );
}
