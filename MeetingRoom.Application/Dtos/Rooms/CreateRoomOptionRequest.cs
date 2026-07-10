namespace MeetingRoom.Application.Dtos.Rooms
{
    /// <summary>
    /// Містить дані для створення додаткової послуги залу.
    /// </summary>
    public class CreateRoomOptionRequest
    {
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
