using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;

namespace Abp.MeetingRoom.Bll.Common.RoomBookings;

/// <summary>
/// Визначає операції доступу до даних бронювань конференц-залів.
/// </summary>
public interface IRoomBookingRepository
{
    /// <summary>
    /// Отримує всі бронювання конференц-залів.
    /// </summary>
    /// <returns>Список бронювань.</returns>
    Task<List<RoomBooking>> GetAllAsync();

    /// <summary>
    /// Отримує бронювання за його ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання.</param>
    /// <returns>Знайдене бронювання або <see langword="null" />, якщо воно не існує.</returns>
    Task<RoomBooking?> GetByIdAsync(Guid id);

    /// <summary>
    /// Створює бронювання конференц-залу з вибраними послугами.
    /// </summary>
    /// <param name="roomId">Унікальний ідентифікатор залу.</param>
    /// <param name="startTime">Дата й час початку бронювання в UTC.</param>
    /// <param name="endTime">Дата й час завершення бронювання в UTC.</param>
    /// <param name="selectedOptionIds">Ідентифікатори вибраних додаткових послуг.</param>
    /// <returns>Створене бронювання.</returns>
    Task<RoomBooking> CreateAsync(
        Guid roomId,
        DateTime startTime,
        DateTime endTime,
        IReadOnlyCollection<Guid> selectedOptionIds
    );

    /// <summary>
    /// Скасовує бронювання конференц-залу.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання.</param>
    /// <returns><see langword="true" />, якщо бронювання скасовано; інакше — <see langword="false" />.</returns>
    Task<bool> CancelAsync(Guid id);

    /// <summary>
    /// Отримує бронювання, що належать до вказаного періоду.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Список бронювань за період.</returns>
    Task<List<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to);

    /// <summary>
    /// Отримує активні бронювання, що належать до вказаного періоду.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Список активних бронювань за період.</returns>
    Task<List<RoomBooking>> GetActiveByPeriodAsync(DateTime from, DateTime to);
}
