namespace MeetingRoom.Application.Dtos.Reports
{
    /// <summary>
    /// Містить інформацію про популярність додаткової послуги.
    /// </summary>
    public class PopularOptionReportResponse
    {
        /// <summary>
        /// Назва додаткової послуги.
        /// </summary>
        public string OptionName { get; set; } = string.Empty;

        /// <summary>
        /// Кількість використань послуги у бронюваннях.
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>
        /// Загальний дохід від послуги.
        /// </summary>
        public decimal Revenue { get; set; }
    }
}
