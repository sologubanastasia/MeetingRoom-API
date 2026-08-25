namespace Abp.MeetingRoom.Services.Web.Reports.Dtos
{
    public class RoomUsageReportResponse
    {
        public string RoomName { get; set; } = string.Empty;
        public int BookingsCount { get; set; }
        public double BookedHours { get; set; }
        public decimal Revenue { get; set; }
    }
}
