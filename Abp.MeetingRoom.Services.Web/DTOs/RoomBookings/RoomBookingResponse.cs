using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using System.Text.Json.Serialization;

namespace Abp.MeetingRoom.Services.Web.DTOs.RoomBookings
{
    /// <summary>
    /// Представляє бронювання конференц-залу у відповіді API.
    /// </summary>
    public class RoomBookingResponse
    {
        /// <summary>
        /// Отримує або задає унікальний ідентифікатор бронювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Отримує або задає ідентифікатор заброньованого залу.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Отримує або задає назву заброньованого залу.
        /// </summary>
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає дату й час початку бронювання в UTC.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Отримує або задає дату й час завершення бронювання в UTC.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Отримує або задає розраховану вартість оренди залу.
        /// </summary>
        public decimal RoomPrice { get; set; }

        /// <summary>
        /// Отримує або задає загальну вартість вибраних послуг.
        /// </summary>
        public decimal OptionsPrice { get; set; }

        /// <summary>
        /// Отримує або задає повну вартість бронювання.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Отримує або задає поточний статус бронювання.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Отримує або задає дату й час створення бронювання в UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Отримує або задає вибрані додаткові послуги.
        /// </summary>
        public List<BookingOptionResponse> SelectedOptions { get; set; } = new();
    }
}
