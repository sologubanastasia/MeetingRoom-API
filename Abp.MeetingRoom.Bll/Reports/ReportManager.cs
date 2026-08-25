using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Bll.Common.Reports.Models;
namespace Abp.MeetingRoom.Bll.Reports;
public sealed class ReportManager : IReportManager
{
    private readonly IReportRepository _reportRepository;
    public ReportManager(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    public async Task<RevenueReport> GetRevenueReportAsync(DateTime from, DateTime to)
    {
        ValidatePeriod(from, to);
        return await _reportRepository.GetRevenueAsync(from, to);
    }
    public async Task<IReadOnlyList<PopularOptionReport>> GetPopularOptionsReportAsync(
        DateTime from,
        DateTime to
    )
    {
        ValidatePeriod(from, to);
        return await _reportRepository.GetPopularOptionsAsync(from, to);
    }
    public async Task<IReadOnlyList<RoomUsageReport>> GetRoomUsageReportAsync(
        DateTime from,
        DateTime to
    )
    {
        ValidatePeriod(from, to);
        return await _reportRepository.GetRoomUsageAsync(from, to);
    }
    private static void ValidatePeriod(DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentException("From date must be earlier than to date.");
        }
    }
}
