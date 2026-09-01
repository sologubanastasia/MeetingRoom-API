namespace Abp.MeetingRoom.Services.Web.DTOs.Reports
{
    /// <summary>
    /// Представляє статистику використання конференц-залу за вибраний період.
    /// </summary>
    public class RoomUsageReportResponse
    {
        /// <summary>
        /// Отримує або задає назву конференц-залу.
        /// </summary>
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає кількість бронювань залу.
        /// </summary>
        public int BookingsCount { get; set; }

        /// <summary>
        /// Отримує або задає загальну кількість заброньованих годин.
        /// </summary>
        public double BookedHours { get; set; }

        /// <summary>
        /// Отримує або задає дохід, отриманий від використання залу.
        /// </summary>
        public decimal Revenue { get; set; }
    }
}
