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
    public async Task<RevenueReport> GetRevenueReportAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        ValidatePeriod(from, to);
        return await _reportRepository.GetRevenueAsync(from, to, cancellationToken);
    }
    public async Task<IReadOnlyList<PopularOptionReport>> GetPopularOptionsReportAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        ValidatePeriod(from, to);
        return await _reportRepository.GetPopularOptionsAsync(
            from,
            to,
            cancellationToken
        );
    }
    public async Task<IReadOnlyList<RoomUsageReport>> GetRoomUsageReportAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        ValidatePeriod(from, to);
        return await _reportRepository.GetRoomUsageAsync(from, to, cancellationToken);
    }
    private static void ValidatePeriod(DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentException("From date must be earlier than to date.");
        }
    }
}
