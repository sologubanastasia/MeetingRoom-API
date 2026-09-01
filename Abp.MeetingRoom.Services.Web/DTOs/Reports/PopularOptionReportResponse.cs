namespace Abp.MeetingRoom.Services.Web.DTOs.Reports
{
    /// <summary>
    /// Представляє статистику використання додаткової послуги.
    /// </summary>
    public class PopularOptionReportResponse
    {
        /// <summary>
        /// Отримує або задає назву додаткової послуги.
        /// </summary>
        public string OptionName { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає кількість використань послуги.
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>
        /// Отримує або задає дохід від послуги.
        /// </summary>
        public decimal Revenue { get; set; }
    }
}
