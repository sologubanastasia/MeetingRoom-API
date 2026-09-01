namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    /// <summary>
    /// Містить дані додаткової послуги під час створення конференц-залу.
    /// </summary>
    public class CreateRoomOptionRequest
    {
        /// <summary>
        /// Отримує або задає назву додаткової послуги.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає вартість додаткової послуги.
        /// </summary>
        public decimal Price { get; set; }
    }
}
