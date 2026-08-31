namespace Abp.MeetingRoom.Services.Web.DTOs.RoomBookings
{
    /// <summary>
    /// Містить дані для створення бронювання конференц-залу.
    /// </summary>
    public class CreateRoomBookingRequest
    {
        /// <summary>
        /// Отримує або задає ідентифікатор конференц-залу.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Отримує або задає дату й час початку бронювання в UTC.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Отримує або задає дату й час завершення бронювання в UTC.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Отримує або задає ідентифікатори вибраних додаткових послуг.
        /// </summary>
        public List<Guid> SelectedOptionIds { get; set; } = new();
    }
}
