using MeetingRoom.Application.Dtos.Reports;
using MeetingRoom.Application.Services.RoomBooking;
using MeetingRoom.Domain.Entities;

namespace MeetingRoom.Application.Services.Report
{
    /// <summary>
    /// Формує бізнес-звіти на основі даних бронювань.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IRoomBookingService _roomBookingService;

        public ReportService(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        public async Task<RevenueReportResponse> GetRevenueReportAsync(DateTime from, DateTime to)
        {
            var bookings = await _roomBookingService.GetByPeriodAsync(from, to);

            var activeBookings = bookings
                .Where(booking => booking.Status == BookingStatus.Active)
                .ToList();

            return new RevenueReportResponse
            {
                BookingsCount = activeBookings.Count,
                RoomRevenue = activeBookings.Sum(booking => booking.RoomPrice),
                OptionsRevenue = activeBookings.Sum(booking => booking.OptionsPrice),
                TotalRevenue = activeBookings.Sum(booking => booking.TotalPrice),
            };
        }

        public async Task<List<PopularOptionReportResponse>> GetPopularOptionsReportAsync(
            DateTime from,
            DateTime to
        )
        {
            var bookings = await _roomBookingService.GetByPeriodAsync(from, to);

            return bookings
                .Where(booking => booking.Status == BookingStatus.Active)
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

        public async Task<List<RoomUsageReportResponse>> GetRoomUsageReportAsync(
            DateTime from,
            DateTime to
        )
        {
            var bookings = await _roomBookingService.GetByPeriodAsync(from, to);

            return bookings
                .Where(booking => booking.Status == BookingStatus.Active)
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
