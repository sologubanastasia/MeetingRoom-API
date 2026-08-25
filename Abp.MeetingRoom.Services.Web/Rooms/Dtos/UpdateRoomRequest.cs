namespace Abp.MeetingRoom.Services.Web.Rooms.Dtos
{
    public class UpdateRoomRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerHour { get; set; }
        public List<UpdateRoomOptionRequest> Options { get; set; } = new();
    }
}
