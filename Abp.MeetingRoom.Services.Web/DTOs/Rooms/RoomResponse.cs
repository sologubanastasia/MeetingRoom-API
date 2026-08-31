namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    public class RoomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerHour { get; set; }
        public List<RoomOptionResponse> Options { get; set; } = new();
    }
}
