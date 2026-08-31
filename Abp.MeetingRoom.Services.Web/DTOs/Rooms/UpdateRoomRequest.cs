namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    /// <summary>
    /// Містить дані для оновлення конференц-залу.
    /// </summary>
    public class UpdateRoomRequest
    {
        /// <summary>
        /// Отримує або задає нову назву конференц-залу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Отримує або задає нову максимальну кількість осіб у залі.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Отримує або задає нову базову вартість оренди залу за годину.
        /// </summary>
        public decimal PricePerHour { get; set; }

        /// <summary>
        /// Отримує або задає актуальний набір додаткових послуг залу.
        /// </summary>
        public List<UpdateRoomOptionRequest> Options { get; set; } = new();
    }
}
