using MeetingRoom.Domain.Entities;
using System.Text.Json.Serialization;

namespace MeetingRoom.Application.Dtos.RoomBookings
{
    /// <summary>
    /// Містить повну інформацію про бронювання конференц-залу.
    /// </summary>
    public class RoomBookingResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор бронювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Унікальний ідентифікатор конференц-залу.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Назва конференц-залу.
        /// </summary>
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// Дата і час початку бронювання.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Дата і час завершення бронювання.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Вартість оренди залу.
        /// </summary>
        public decimal RoomPrice { get; set; }

        /// <summary>
        /// Загальна вартість додаткових послуг.
        /// </summary>
        public decimal OptionsPrice { get; set; }

        /// <summary>
        /// Загальна вартість бронювання.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Поточний статус бронювання.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Дата і час створення бронювання.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Список вибраних додаткових послуг.
        /// </summary>
        public List<BookingOptionResponse> SelectedOptions { get; set; } = new();
    }
}
