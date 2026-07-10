namespace MeetingRoom.Application.Dtos.RoomBookings
{
    /// <summary>
    /// Містить інформацію про додаткову послугу,
    /// вибрану під час бронювання.
    /// </summary>
    public class BookingOptionResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор запису послуги бронювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Унікальний ідентифікатор послуги залу.
        /// </summary>
        public Guid RoomOptionId { get; set; }

        /// <summary>
        /// Назва вибраної послуги.
        /// </summary>
        public string OptionName { get; set; } = string.Empty;

        /// <summary>
        /// Вартість вибраної послуги.
        /// </summary>
        public decimal OptionPrice { get; set; }
    }
}
