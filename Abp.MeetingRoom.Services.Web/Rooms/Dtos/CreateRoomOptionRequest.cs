namespace Abp.MeetingRoom.Services.Web.Rooms.Dtos
{
    public class CreateRoomOptionRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
