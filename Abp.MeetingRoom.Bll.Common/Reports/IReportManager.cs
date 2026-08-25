using Abp.MeetingRoom.Bll.Common.Reports.Models;
namespace Abp.MeetingRoom.Bll.Common.Reports;
public interface IReportManager
{
    Task<RevenueReport> GetRevenueReportAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<PopularOptionReport>> GetPopularOptionsReportAsync(
        DateTime from,
        DateTime to
    );
    Task<IReadOnlyList<RoomUsageReport>> GetRoomUsageReportAsync(DateTime from, DateTime to);
}
