namespace Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Entities;
internal sealed class BookingOptionEntity
{
    public Guid Id { get; init; }
    public Guid RoomBookingId { get; init; }
    public Guid RoomOptionId { get; init; }
    public string OptionName { get; init; } = string.Empty;
    public decimal OptionPrice { get; init; }
}
