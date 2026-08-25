using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
namespace Abp.MeetingRoom.Bll.Common.Rooms.Models;
public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal PricePerHour { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<RoomOption> Options { get; set; } = new List<RoomOption>();
    public ICollection<RoomBooking> Bookings { get; set; } = new List<RoomBooking>();
}
