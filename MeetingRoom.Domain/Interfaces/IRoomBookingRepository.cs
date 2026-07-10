using MeetingRoom.Domain.Entities;

namespace MeetingRoom.Domain.Interfaces;

/// <summary>
/// Визначає операції доступу до даних бронювань.
/// </summary>
public interface IRoomBookingRepository : IBaseRepository<RoomBooking>
{
    /// <summary>
    /// Повертає бронювання за вказаний період.
    /// </summary>
    /// <param name="from">Початок періоду.</param>
    /// <param name="to">Кінець періоду.</param>
    /// <returns>Список бронювань, що входять у заданий період.</returns>
    Task<List<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to);

    /// <summary>
    /// Перевіряє наявність конфлікту бронювання для залу.
    /// </summary>
    /// <param name="roomId">Ідентифікатор залу.</param>
    /// <param name="startTime">Час початку нового бронювання.</param>
    /// <param name="endTime">Час завершення нового бронювання.</param>
    /// <returns>
    /// True, якщо існує активне бронювання, що перетинається
    /// із заданим періодом.
    /// </returns>
    Task<bool> HasTimeConflictAsync(Guid roomId, DateTime startTime, DateTime endTime);
}
