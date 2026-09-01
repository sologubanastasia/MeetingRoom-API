namespace Abp.MeetingRoom.Services.Web.DTOs.Rooms
{
    /// <summary>
    /// Містить дані для створення конференц-залу.
    /// </summary>
    public class CreateRoomRequest
    {
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
        /// Отримує або задає додаткові послуги, доступні для залу.
        /// </summary>
        public List<CreateRoomOptionRequest> Options { get; set; } = new();
    }
}
