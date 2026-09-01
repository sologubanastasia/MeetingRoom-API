using Abp.MeetingRoom.Bll.Common.Reports.Models;
using Abp.MeetingRoom.Services.Web.DTOs.Reports;
using AutoMapper;

namespace Abp.MeetingRoom.Services.Web.MappingProfiles;

public sealed class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {
        CreateMap<RevenueReport, RevenueReportResponse>();
        CreateMap<PopularOptionReport, PopularOptionReportResponse>();
        CreateMap<RoomUsageReport, RoomUsageReportResponse>();
    }
}
