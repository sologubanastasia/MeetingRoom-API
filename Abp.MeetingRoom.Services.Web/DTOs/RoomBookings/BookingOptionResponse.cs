namespace Abp.MeetingRoom.Services.Web.DTOs.RoomBookings
{
    public class BookingOptionResponse
    {
        public Guid Id { get; set; }
        public Guid RoomOptionId { get; set; }
        public string OptionName { get; set; } = string.Empty;
        public decimal OptionPrice { get; set; }
    }
}
