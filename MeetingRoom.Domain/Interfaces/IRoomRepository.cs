using MeetingRoom.Domain.Entities;

namespace MeetingRoom.Domain.Interfaces;

/// <summary>
/// Визначає операції доступу до даних конференц-залів.
/// </summary>
public interface IRoomRepository : IBaseRepository<Room>
{
    /// <summary>
    /// Повертає доступні конференц-зали за заданими параметрами.
    /// </summary>
    /// <param name="startTime">Час початку бронювання.</param>
    /// <param name="endTime">Час завершення бронювання.</param>
    /// <param name="capacity">Мінімальна необхідна місткість.</param>
    /// <returns>
    /// Список залів, які мають достатню місткість
    /// і не зайняті у заданий період.
    /// </returns>
    Task<List<Room>> GetAvailableAsync(DateTime startTime, DateTime endTime, int capacity);
}
