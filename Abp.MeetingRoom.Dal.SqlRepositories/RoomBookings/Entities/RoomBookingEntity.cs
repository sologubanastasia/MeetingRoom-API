using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
namespace Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Entities;
internal sealed class RoomBookingEntity
{
    public Guid Id { get; init; }
    public Guid RoomId { get; init; }
    public string RoomName { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public decimal RoomPrice { get; init; }
    public decimal OptionsPrice { get; init; }
    public decimal TotalPrice { get; init; }
    public BookingStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<BookingOptionEntity> SelectedOptions { get; } = new();
}
