namespace Abp.MeetingRoom.Services.Web.DTOs.Reports
{
    public class PopularOptionReportResponse
    {
        public string OptionName { get; set; } = string.Empty;
        public int UsageCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
