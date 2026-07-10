using MeetingRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoom.Infrastructure.Configurations;

public class RoomOptionConfiguration : IEntityTypeConfiguration<RoomOption>
{
    public void Configure(EntityTypeBuilder<RoomOption> builder)
    {
        builder.HasKey(option => option.Id);

        builder.Property(option => option.RoomId).IsRequired();

        builder.Property(option => option.Name).IsRequired().HasMaxLength(100);

        builder.Property(option => option.Price).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(option => option.IsActive).HasDefaultValue(true);

        builder
            .HasOne(option => option.Room)
            .WithMany(room => room.Options)
            .HasForeignKey(option => option.RoomId);

        builder.HasData(SeedData.RoomOptions);
    }
}
