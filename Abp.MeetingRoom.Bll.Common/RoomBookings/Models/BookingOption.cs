namespace Abp.MeetingRoom.Bll.Common.RoomBookings.Models
{
    public class BookingOption
    {
        public Guid Id { get; set; }
        public Guid RoomBookingId { get; set; }
        public RoomBooking RoomBooking { get; set; } = null!;
        public Guid RoomOptionId { get; set; }
        public string OptionName { get; set; } = null!;
        public decimal OptionPrice { get; set; }
    }
    public enum BookingStatus
    {
        Active = 1,
        Cancelled = 2,
    }
}
