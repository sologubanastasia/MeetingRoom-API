namespace MeetingRoom.Application.Dtos.Reports
{
    /// <summary>
    /// Містить інформацію про дохід за вибраний період.
    /// </summary>
    public class RevenueReportResponse
    {
        /// <summary>
        /// Кількість активних бронювань.
        /// </summary>
        public int BookingsCount { get; set; }

        /// <summary>
        /// Загальний дохід від оренди конференц-залів.
        /// </summary>
        public decimal RoomRevenue { get; set; }

        /// <summary>
        /// Загальний дохід від додаткових послуг.
        /// </summary>
        public decimal OptionsRevenue { get; set; }

        /// <summary>
        /// Загальний дохід від залів і додаткових послуг.
        /// </summary>
        public decimal TotalRevenue { get; set; }
    }
}
