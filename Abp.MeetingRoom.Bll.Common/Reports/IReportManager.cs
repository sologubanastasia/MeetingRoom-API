using Abp.MeetingRoom.Bll.Common.Reports.Models;

namespace Abp.MeetingRoom.Bll.Common.Reports;

/// <summary>
/// Визначає бізнес-операції для формування звітів.
/// </summary>
public interface IReportManager
{
    /// <summary>
    /// Формує зведений звіт про дохід за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Зведений звіт про дохід.</returns>
    Task<RevenueReport> GetRevenueReportAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Формує звіт про популярність додаткових послуг за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Колекція показників використання додаткових послуг.</returns>
    Task<IReadOnlyList<PopularOptionReport>> GetPopularOptionsReportAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Формує звіт про використання конференц-залів за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>Колекція показників використання конференц-залів.</returns>
    Task<IReadOnlyList<RoomUsageReport>> GetRoomUsageReportAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    );
}
