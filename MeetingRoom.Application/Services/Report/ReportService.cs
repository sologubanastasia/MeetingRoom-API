using MeetingRoom.Application.Dtos.Reports;
using MeetingRoom.Application.Services.RoomBooking;

namespace MeetingRoom.Application.Services.Report
{
    /// <summary>
    /// Формує бізнес-звіти на основі даних бронювань.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IRoomBookingService _roomBookingService;

        /// <summary>
        /// Ініціалізує сервіс формування звітів.
        /// </summary>
        /// <param name="roomBookingService">Сервіс керування бронюваннями.</param>
        public ReportService(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        /// <summary>
        /// Формує звіт про дохід за вказаний період.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Звіт про дохід від активних бронювань.</returns>
        public async Task<RevenueReportResponse> GetRevenueReportAsync(DateTime from, DateTime to)
        {
            var activeBookings = await _roomBookingService.GetActiveByPeriodAsync(from, to);

            return new RevenueReportResponse
            {
                BookingsCount = activeBookings.Count,
                RoomRevenue = activeBookings.Sum(booking => booking.RoomPrice),
                OptionsRevenue = activeBookings.Sum(booking => booking.OptionsPrice),
                TotalRevenue = activeBookings.Sum(booking => booking.TotalPrice),
            };
        }

        /// <summary>
        /// Формує звіт про популярність додаткових послуг за вказаний період.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Список додаткових послуг, упорядкований за популярністю.</returns>
        public async Task<List<PopularOptionReportResponse>> GetPopularOptionsReportAsync(
            DateTime from,
            DateTime to
        )
        {
            var bookings = await _roomBookingService.GetActiveByPeriodAsync(from, to);

            return bookings
                .SelectMany(booking => booking.SelectedOptions)
                .GroupBy(option => option.OptionName)
                .Select(group => new PopularOptionReportResponse
                {
                    OptionName = group.Key,
                    UsageCount = group.Count(),
                    Revenue = group.Sum(option => option.OptionPrice),
                })
                .OrderByDescending(report => report.UsageCount)
                .ToList();
        }

        /// <summary>
        /// Формує звіт про використання конференц-залів за вказаний період.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Список показників використання конференц-залів.</returns>
        public async Task<List<RoomUsageReportResponse>> GetRoomUsageReportAsync(
            DateTime from,
            DateTime to
        )
        {
            var bookings = await _roomBookingService.GetActiveByPeriodAsync(from, to);

            return bookings
                .GroupBy(booking => booking.RoomName)
                .Select(group => new RoomUsageReportResponse
                {
                    RoomName = group.Key,
                    BookingsCount = group.Count(),
                    BookedHours = group.Sum(booking =>
                        (booking.EndTime - booking.StartTime).TotalHours
                    ),
                    Revenue = group.Sum(booking => booking.TotalPrice),
                })
                .OrderByDescending(report => report.BookingsCount)
                .ToList();
        }
    }
}
