namespace Abp.MeetingRoom.Bll.Common.Reports.Models;
public sealed class RoomUsageReport
{
    public string RoomName { get; set; } = string.Empty;
    public int BookingsCount { get; set; }
    public double BookedHours { get; set; }
    public decimal Revenue { get; set; }
}
