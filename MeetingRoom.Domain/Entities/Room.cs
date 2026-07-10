namespace MeetingRoom.Domain.Entities;

/// <summary>
/// Представляє конференц-зал.
/// </summary>
public class Room
{
    /// <summary>
    /// Унікальний ідентифікатор залу.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Назва конференц-залу.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Максимальна кількість людей у залі.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Базова вартість оренди за одну годину.
    /// </summary>
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// Вказує, чи був зал видалений.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Дата і час створення залу.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Список доступних додаткових послуг.
    /// </summary>
    public ICollection<RoomOption> Options { get; set; } = new List<RoomOption>();

    /// <summary>
    /// Список бронювань залу.
    /// </summary>
    public ICollection<RoomBooking> Bookings { get; set; } = new List<RoomBooking>();
}
