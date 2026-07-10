namespace MeetingRoom.Domain.Entities
{
    /// <summary>
    /// Представляє додаткову послугу, вибрану під час бронювання.
    /// </summary>
    public class BookingOption
    {
        /// <summary>
        /// Унікальний ідентифікатор запису.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ідентифікатор бронювання.
        /// </summary>
        public Guid RoomBookingId { get; set; }

        /// <summary>
        /// Бронювання, до якого належить послуга.
        /// </summary>
        public RoomBooking RoomBooking { get; set; } = null!;

        /// <summary>
        /// Ідентифікатор послуги конференц-залу.
        /// </summary>
        public Guid RoomOptionId { get; set; }

        /// <summary>
        /// Назва послуги на момент бронювання.
        /// </summary>
        public string OptionName { get; set; } = null!;

        /// <summary>
        /// Вартість послуги на момент бронювання.
        /// </summary>
        public decimal OptionPrice { get; set; }
    }

    /// <summary>
    /// Визначає статус бронювання.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Активне бронювання.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Скасоване бронювання.
        /// </summary>
        Cancelled = 2,
    }
}
