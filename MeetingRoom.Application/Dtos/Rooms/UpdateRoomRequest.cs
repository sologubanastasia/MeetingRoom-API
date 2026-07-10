namespace MeetingRoom.Application.Dtos.Rooms
{
    /// <summary>
    /// Містить дані для оновлення конференц-залу.
    /// </summary>
    public class UpdateRoomRequest
    {
        /// <summary>
        /// Оновлена назва конференц-залу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Оновлена місткість конференц-залу.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Оновлена вартість оренди за одну годину.
        /// </summary>
        public decimal PricePerHour { get; set; }

        /// <summary>
        /// Оновлений список додаткових послуг.
        /// </summary>
        public List<UpdateRoomOptionRequest> Options { get; set; } = new();
    }
}
