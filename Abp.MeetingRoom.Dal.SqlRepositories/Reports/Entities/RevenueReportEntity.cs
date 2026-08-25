namespace Abp.MeetingRoom.Dal.SqlRepositories.Reports.Entities;
internal sealed class RevenueReportEntity
{
    public int BookingsCount { get; init; }
    public decimal RoomRevenue { get; init; }
    public decimal OptionsRevenue { get; init; }
    public decimal TotalRevenue { get; init; }
}
