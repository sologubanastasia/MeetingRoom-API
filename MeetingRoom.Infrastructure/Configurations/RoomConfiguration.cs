namespace MeetingRoom.Infrastructure.Configurations;

using MeetingRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Capacity).IsRequired();

        builder.Property(x => x.PricePerHour).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder
            .HasMany(x => x.Options)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Bookings)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(SeedData.Rooms);
    }
}
