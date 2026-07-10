using MeetingRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoom.Infrastructure.Configurations;

public class RoomBookingConfiguration : IEntityTypeConfiguration<RoomBooking>
{
    public void Configure(EntityTypeBuilder<RoomBooking> builder)
    {
        builder.ToTable("RoomBookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime).IsRequired();

        builder.Property(x => x.EndTime).IsRequired();

        builder.Property(x => x.RoomPrice).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(x => x.OptionsPrice).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(x => x.CreatedAt).IsRequired();

        builder
            .Property(x => x.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(x => x.Status).IsRequired().HasConversion<int>();

        builder
            .HasMany(x => x.SelectedOptions)
            .WithOne(x => x.RoomBooking)
            .HasForeignKey(x => x.RoomBookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
