using Abp.MeetingRoom.Bll.Common.Reports.Models;

namespace Abp.MeetingRoom.Bll.Common.Reports;

/// <summary>
/// Визначає операції доступу до даних звітів.
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Отримує зведені показники доходу за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Зведений звіт про дохід.</returns>
    Task<RevenueReport> GetRevenueAsync(DateTime from, DateTime to);

    /// <summary>
    /// Отримує статистику популярності додаткових послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Список показників використання додаткових послуг.</returns>
    Task<List<PopularOptionReport>> GetPopularOptionsAsync(DateTime from, DateTime to);

    /// <summary>
    /// Отримує статистику використання конференц-залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Список показників використання конференц-залів.</returns>
    Task<List<RoomUsageReport>> GetRoomUsageAsync(DateTime from, DateTime to);
}
