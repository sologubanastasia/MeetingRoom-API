namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    public class CreateRoomRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerHour { get; set; }
        public List<CreateRoomOptionRequest> Options { get; set; } = new();
    }
}
