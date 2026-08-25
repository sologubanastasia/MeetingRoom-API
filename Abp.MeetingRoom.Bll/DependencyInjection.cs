using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Bll.Reports;
using Abp.MeetingRoom.Bll.RoomBookings;
using Abp.MeetingRoom.Bll.Rooms;
using Microsoft.Extensions.DependencyInjection;
namespace Abp.MeetingRoom.Bll;
public static class DependencyInjection
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services.AddScoped<IRoomManager, RoomManager>();
        services.AddScoped<IRoomBookingManager, RoomBookingManager>();
        services.AddScoped<IReportManager, ReportManager>();
        return services;
    }
}
