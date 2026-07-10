namespace MeetingRoom.Infrastructure;

using MeetingRoom.Domain.Entities;
using MeetingRoom.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

public class RoomDbContext : DbContext
{
    public RoomDbContext(DbContextOptions<RoomDbContext> options)
        : base(options) { }

    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomOption> RoomOptions { get; set; } = null!;
    public DbSet<RoomBooking> RoomBookings { get; set; } = null!;
    public DbSet<BookingOption> BookingOptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(RoomDbContext).Assembly);
    }
}
