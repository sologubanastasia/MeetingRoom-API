namespace Abp.MeetingRoom.Bll.Common.Reports.Models;
public sealed class RevenueReport
{
    public int BookingsCount { get; set; }
    public decimal RoomRevenue { get; set; }
    public decimal OptionsRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
}
