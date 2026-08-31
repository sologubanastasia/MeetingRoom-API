namespace Abp.MeetingRoom.Services.Web.DTOs.Reports
{
    public class RevenueReportResponse
    {
        public int BookingsCount { get; set; }
        public decimal RoomRevenue { get; set; }
        public decimal OptionsRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
