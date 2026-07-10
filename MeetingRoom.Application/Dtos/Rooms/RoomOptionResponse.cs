namespace MeetingRoom.Application.Dtos.Rooms
{
    /// <summary>
    /// Містить інформацію про додаткову послугу конференц-залу.
    /// </summary>
    public class RoomOptionResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор послуги.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва додаткової послуги.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Вартість додаткової послуги.
        /// </summary>
        public decimal Price { get; set; }
    }
}
