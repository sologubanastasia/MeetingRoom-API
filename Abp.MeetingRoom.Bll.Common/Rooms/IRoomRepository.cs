using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;

namespace Abp.MeetingRoom.Bll.Common.Rooms;

/// <summary>
/// Визначає операції доступу до даних конференц-залів.
/// </summary>
public interface IRoomRepository
{
    /// <summary>
    /// Отримує всі активні конференц-зали.
    /// </summary>
    /// <returns>Список активних конференц-залів.</returns>
    Task<List<Room>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Отримує конференц-зал за його ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <returns>Знайдений зал або <see langword="null" />, якщо зал не існує.</returns>
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Створює конференц-зал разом із його додатковими послугами.
    /// </summary>
    /// <param name="room">Дані конференц-залу.</param>
    /// <returns>Створений конференц-зал.</returns>
    Task<Room> CreateAsync(Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Оновлює конференц-зал і набір його додаткових послуг.
    /// </summary>
    /// <param name="room">Актуальні дані конференц-залу.</param>
    /// <returns>Оновлений конференц-зал.</returns>
    Task<Room> UpdateAsync(Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Позначає конференц-зал як видалений без фізичного видалення запису.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <returns><see langword="true" />, якщо зал видалено; інакше — <see langword="false" />.</returns>
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Отримує зали, доступні для бронювання у вказаний період.
    /// </summary>
    /// <param name="startTime">Дата й час початку періоду в UTC.</param>
    /// <param name="endTime">Дата й час завершення періоду в UTC.</param>
    /// <param name="capacity">Мінімальна необхідна місткість залу.</param>
    /// <returns>Список доступних конференц-залів.</returns>
    Task<List<Room>> GetAvailableAsync(
        DateTime startTime,
        DateTime endTime,
        int capacity,
        CancellationToken cancellationToken
    );
}
