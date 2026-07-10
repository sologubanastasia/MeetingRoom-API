namespace MeetingRoom.Application.Dtos.Rooms
{
    /// <summary>
    /// Містить дані для створення конференц-залу.
    /// </summary>
    public class CreateRoomRequest
    {
        /// <summary>
        /// Назва конференц-залу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Максимальна кількість людей у залі.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Базова вартість оренди залу за одну годину.
        /// </summary>
        public decimal PricePerHour { get; set; }

        /// <summary>
        /// Список доступних додаткових послуг.
        /// </summary>
        public List<CreateRoomOptionRequest> Options { get; set; } = new();
    }
}
