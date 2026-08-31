namespace Abp.MeetingRoom.Services.Web.DTOs.RoomBookings
{
    /// <summary>
    /// Представляє вибрану додаткову послугу бронювання у відповіді API.
    /// </summary>
    public class BookingOptionResponse
    {
        /// <summary>
        /// Отримує або задає унікальний ідентифікатор запису послуги бронювання.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Отримує або задає ідентифікатор послуги конференц-залу.
        /// </summary>
        public Guid RoomOptionId { get; set; }

        /// <summary>
        /// Отримує або задає назву послуги на момент бронювання.
        /// </summary>
        public string OptionName { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає вартість послуги на момент бронювання.
        /// </summary>
        public decimal OptionPrice { get; set; }
    }
}
