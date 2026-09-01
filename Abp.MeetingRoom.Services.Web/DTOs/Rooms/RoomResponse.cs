namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    /// <summary>
    /// Представляє конференц-зал у відповіді API.
    /// </summary>
    public class RoomResponse
    {
        /// <summary>
        /// Отримує або задає унікальний ідентифікатор залу.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Отримує або задає назву конференц-залу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає максимальну кількість осіб у залі.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Отримує або задає базову вартість оренди залу за годину.
        /// </summary>
        public decimal PricePerHour { get; set; }

        /// <summary>
        /// Отримує або задає доступні додаткові послуги залу.
        /// </summary>
        public List<RoomOptionResponse> Options { get; set; } = new();
    }
}
