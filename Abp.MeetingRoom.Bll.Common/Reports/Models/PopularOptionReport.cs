namespace Abp.MeetingRoom.Bll.Common.Reports.Models;
public sealed class PopularOptionReport
{
    public string OptionName { get; set; } = string.Empty;
    public int UsageCount { get; set; }
    public decimal Revenue { get; set; }
}
