using Abp.MeetingRoom.Bll.Common.Reports.Models;
using Abp.MeetingRoom.Services.Web.Reports.Dtos;
namespace Abp.MeetingRoom.Services.Web.Reports.Mappings;
public static class ReportMapper
{
    public static RevenueReportResponse ToResponse(RevenueReport report)
    {
        return new RevenueReportResponse
        {
            BookingsCount = report.BookingsCount,
            RoomRevenue = report.RoomRevenue,
            OptionsRevenue = report.OptionsRevenue,
            TotalRevenue = report.TotalRevenue,
        };
    }
    public static PopularOptionReportResponse ToResponse(PopularOptionReport report)
    {
        return new PopularOptionReportResponse
        {
            OptionName = report.OptionName,
            UsageCount = report.UsageCount,
            Revenue = report.Revenue,
        };
    }
    public static RoomUsageReportResponse ToResponse(RoomUsageReport report)
    {
        return new RoomUsageReportResponse
        {
            RoomName = report.RoomName,
            BookingsCount = report.BookingsCount,
            BookedHours = report.BookedHours,
            Revenue = report.Revenue,
        };
    }
}
