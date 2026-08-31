namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    public class RoomOptionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
