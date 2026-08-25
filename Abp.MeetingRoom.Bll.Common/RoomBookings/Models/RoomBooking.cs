using Abp.MeetingRoom.Bll.Common.Rooms.Models;
namespace Abp.MeetingRoom.Bll.Common.RoomBookings.Models
{
    public class RoomBooking
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal OptionsPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<BookingOption> SelectedOptions { get; set; } = new List<BookingOption>();
    }
}
