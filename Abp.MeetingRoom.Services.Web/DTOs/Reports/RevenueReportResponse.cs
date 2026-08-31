namespace Abp.MeetingRoom.Services.Web.DTOs.Reports
{
    /// <summary>
    /// Представляє зведений звіт про дохід за вибраний період.
    /// </summary>
    public class RevenueReportResponse
    {
        /// <summary>
        /// Отримує або задає кількість бронювань за період.
        /// </summary>
        public int BookingsCount { get; set; }

        /// <summary>
        /// Отримує або задає дохід від оренди залів.
        /// </summary>
        public decimal RoomRevenue { get; set; }

        /// <summary>
        /// Отримує або задає дохід від додаткових послуг.
        /// </summary>
        public decimal OptionsRevenue { get; set; }

        /// <summary>
        /// Отримує або задає загальний дохід.
        /// </summary>
        public decimal TotalRevenue { get; set; }
    }
}
