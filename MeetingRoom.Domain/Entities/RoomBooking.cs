namespace MeetingRoom.Domain.Entities
{
    /// <summary>
    /// Представляє бронювання конференц-залу.
    /// </summary>
    public class RoomBooking
    {
        /// <summary>
        /// Унікальний ідентифікатор бронювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ідентифікатор конференц-залу.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Конференц-зал, що був заброньований.
        /// </summary>
        public Room Room { get; set; } = null!;

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
        public BookingStatus Status { get; set; }

        /// <summary>
        /// Дата і час створення бронювання.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Список вибраних додаткових послуг.
        /// </summary>
        public ICollection<BookingOption> SelectedOptions { get; set; } = new List<BookingOption>();
    }
}
