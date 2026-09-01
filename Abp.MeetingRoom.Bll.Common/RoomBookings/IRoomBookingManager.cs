using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;

namespace Abp.MeetingRoom.Bll.Common.RoomBookings;

/// <summary>
/// Визначає бізнес-операції для керування бронюваннями конференц-залів.
/// </summary>
public interface IRoomBookingManager
{
    /// <summary>
    /// Створює нове бронювання конференц-залу.
    /// </summary>
    /// <param name="booking">Дані нового бронювання.</param>
    /// <returns>Створене бронювання з розрахованою вартістю.</returns>
    Task<RoomBooking> CreateRoomBookingAsync(RoomBooking booking);

    /// <summary>
    /// Отримує всі бронювання конференц-залів.
    /// </summary>
    /// <returns>Колекція бронювань.</returns>
    Task<IReadOnlyList<RoomBooking>> GetAllRoomBookingsAsync();

    /// <summary>
    /// Отримує бронювання за його ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання.</param>
    /// <returns>Знайдене бронювання або <see langword="null" />, якщо воно не існує.</returns>
    Task<RoomBooking?> GetRoomBookingByIdAsync(Guid id);

    /// <summary>
    /// Скасовує бронювання конференц-залу.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання.</param>
    /// <returns><see langword="true" />, якщо бронювання скасовано; інакше — <see langword="false" />.</returns>
    Task<bool> CancelRoomBookingAsync(Guid id);

    /// <summary>
    /// Отримує бронювання за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду в UTC.</param>
    /// <param name="to">Завершення періоду в UTC.</param>
    /// <returns>Колекція бронювань за період.</returns>
    Task<IReadOnlyList<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to);

    /// <summary>
    /// Отримує активні бронювання за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду в UTC.</param>
    /// <param name="to">Завершення періоду в UTC.</param>
    /// <returns>Колекція активних бронювань за період.</returns>
    Task<IReadOnlyList<RoomBooking>> GetActiveByPeriodAsync(DateTime from, DateTime to);
}
