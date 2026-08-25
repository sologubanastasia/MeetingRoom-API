namespace Abp.MeetingRoom.Dal.SqlRepositories.Reports.Entities;
internal sealed class PopularOptionReportEntity
{
    public string OptionName { get; init; } = string.Empty;
    public int UsageCount { get; init; }
    public decimal Revenue { get; init; }
}
