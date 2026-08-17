using FluentValidation;
using MeetingRoom.Application.Services.Price;
using MeetingRoom.Application.Services.Report;
using MeetingRoom.Application.Services.RoomBooking;
using MeetingRoom.Application.Services.Rooms;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingRoom.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomBookingService, RoomBookingService>();
        services.AddScoped<IPriceService, PriceService>();
        services.AddScoped<IReportService, ReportService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(configuration => { }, typeof(DependencyInjection).Assembly);

        return services;
    }
}
