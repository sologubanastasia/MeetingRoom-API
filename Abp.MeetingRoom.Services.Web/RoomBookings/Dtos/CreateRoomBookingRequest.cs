namespace Abp.MeetingRoom.Services.Web.RoomBookings.Dtos
{
    public class CreateRoomBookingRequest
    {
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Guid> SelectedOptionIds { get; set; } = new();
    }
}
