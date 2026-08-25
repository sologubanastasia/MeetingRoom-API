using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Dal.SqlRepositories.Reports;
using Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings;
using Abp.MeetingRoom.Dal.SqlRepositories.Rooms;
using Microsoft.Extensions.DependencyInjection;
namespace Abp.MeetingRoom.Dal.SqlRepositories;
public static class DependencyInjection
{
    public static IServiceCollection AddSqlRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        return services;
    }
}
