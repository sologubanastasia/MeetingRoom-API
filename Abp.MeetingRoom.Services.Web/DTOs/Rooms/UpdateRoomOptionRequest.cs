namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    public class UpdateRoomOptionRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
