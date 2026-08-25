namespace Abp.MeetingRoom.Services.Web.Rooms.Dtos
{
    public class UpdateRoomOptionRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
