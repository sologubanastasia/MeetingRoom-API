using MeetingRoom.Domain.Interfaces;
using MeetingRoom.Infrastructure;
using MeetingRoom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingRoom.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<RoomDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();

            return services;
        }
    }
}
