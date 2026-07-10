namespace MeetingRoom.Application.Dtos.Rooms
{
    /// <summary>
    /// Містить інформацію про конференц-зал.
    /// </summary>
    public class RoomResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор залу.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Назва конференц-залу.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Максимальна кількість людей у залі.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Базова вартість оренди за одну годину.
        /// </summary>
        public decimal PricePerHour { get; set; }

        /// <summary>
        /// Список доступних додаткових послуг.
        /// </summary>
        public List<RoomOptionResponse> Options { get; set; } = new();
    }
}
