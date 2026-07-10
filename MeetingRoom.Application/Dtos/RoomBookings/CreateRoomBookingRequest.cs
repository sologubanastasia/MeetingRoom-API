namespace MeetingRoom.Application.Dtos.RoomBookings
{
    /// <summary>
    /// Містить дані для створення бронювання конференц-залу.
    /// </summary>
    public class CreateRoomBookingRequest
    {
        /// <summary>
        /// Унікальний ідентифікатор конференц-залу.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Дата і час початку бронювання.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Дата і час завершення бронювання.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Список ідентифікаторів вибраних додаткових послуг.
        /// </summary>
        public List<Guid> SelectedOptionIds { get; set; } = new();
    }
}
