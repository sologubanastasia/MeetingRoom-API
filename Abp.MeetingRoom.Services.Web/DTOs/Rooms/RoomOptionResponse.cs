namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    /// <summary>
    /// Представляє додаткову послугу конференц-залу у відповіді API.
    /// </summary>
    public class RoomOptionResponse
    {
        /// <summary>
        /// Отримує або задає унікальний ідентифікатор послуги.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Отримує або задає назву послуги.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає вартість послуги.
        /// </summary>
        public decimal Price { get; set; }
    }
}
