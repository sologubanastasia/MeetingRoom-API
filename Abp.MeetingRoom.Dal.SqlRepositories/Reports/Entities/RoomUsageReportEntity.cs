namespace Abp.MeetingRoom.Dal.SqlRepositories.Reports.Entities;
internal sealed class RoomUsageReportEntity
{
    public string RoomName { get; init; } = string.Empty;
    public int BookingsCount { get; init; }
    public double BookedHours { get; init; }
    public decimal Revenue { get; init; }
}
