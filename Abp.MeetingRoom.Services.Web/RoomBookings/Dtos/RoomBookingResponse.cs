using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using System.Text.Json.Serialization;
namespace Abp.MeetingRoom.Services.Web.RoomBookings.Dtos
{
    public class RoomBookingResponse
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal OptionsPrice { get; set; }
        public decimal TotalPrice { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<BookingOptionResponse> SelectedOptions { get; set; } = new();
    }
}
