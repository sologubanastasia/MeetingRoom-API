namespace MeetingRoom.Application.Dtos.Rooms
{
    /// <summary>
    /// Містить дані для оновлення додаткової послуги залу.
    /// </summary>
    public class UpdateRoomOptionRequest
    {
        /// <summary>
        /// Оновлена назва додаткової послуги.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Оновлена вартість додаткової послуги.
        /// </summary>
        public decimal Price { get; set; }
    }
}
