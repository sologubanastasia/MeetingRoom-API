using MeetingRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoom.Infrastructure.Configurations;

public class BookingOptionConfiguration : IEntityTypeConfiguration<BookingOption>
{
    public void Configure(EntityTypeBuilder<BookingOption> builder)
    {
        builder.HasKey(bookingOption => bookingOption.Id);

        builder.Property(bookingOption => bookingOption.RoomBookingId).IsRequired();

        builder.Property(bookingOption => bookingOption.RoomOptionId).IsRequired();

        builder.Property(bookingOption => bookingOption.OptionName).IsRequired().HasMaxLength(100);

        builder
            .Property(bookingOption => bookingOption.OptionPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .HasOne(bookingOption => bookingOption.RoomBooking)
            .WithMany(roomBooking => roomBooking.SelectedOptions)
            .HasForeignKey(bookingOption => bookingOption.RoomBookingId);

        builder
            .HasOne<RoomOption>()
            .WithMany(roomOption => roomOption.BookingOptions)
            .HasForeignKey(bookingOption => bookingOption.RoomOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
