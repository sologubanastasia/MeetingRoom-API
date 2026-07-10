namespace MeetingRoom.Application.Dtos.Reports
{
    /// <summary>
    /// Містить статистику використання конференц-залу.
    /// </summary>
    public class RoomUsageReportResponse
    {
        /// <summary>
        /// Назва конференц-залу.
        /// </summary>
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// Кількість бронювань залу.
        /// </summary>
        public int BookingsCount { get; set; }

        /// <summary>
        /// Загальна кількість заброньованих годин.
        /// </summary>
        public double BookedHours { get; set; }

        /// <summary>
        /// Загальний дохід від бронювань залу.
        /// </summary>
        public decimal Revenue { get; set; }
    }
}
