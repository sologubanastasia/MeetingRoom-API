using Abp.MeetingRoom.Bll.Common.Reports.Models;
namespace Abp.MeetingRoom.Bll.Common.Reports;
public interface IReportRepository
{
    Task<RevenueReport> GetRevenueAsync(DateTime from, DateTime to);
    Task<List<PopularOptionReport>> GetPopularOptionsAsync(DateTime from, DateTime to);
    Task<List<RoomUsageReport>> GetRoomUsageAsync(DateTime from, DateTime to);
}
