using MeetingRoom.Application.Dtos.Reports;

namespace MeetingRoom.Application.Services.Report
{
    /// <summary>
    /// Визначає операції для формування бізнес-звітів.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Формує звіт про дохід за вказаний період.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Звіт про дохід.</returns>
        Task<RevenueReportResponse> GetRevenueReportAsync(DateTime from, DateTime to);

        /// <summary>
        /// Формує звіт про популярність додаткових послуг.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Список популярних послуг.</returns>
        Task<List<PopularOptionReportResponse>> GetPopularOptionsReportAsync(
            DateTime from,
            DateTime to
        );

        /// <summary>
        /// Формує звіт про використання конференц-залів.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Статистика використання залів.</returns>
        Task<List<RoomUsageReportResponse>> GetRoomUsageReportAsync(DateTime from, DateTime to);
    }
}
