namespace MeetingRoom.Domain.Entities;

/// <summary>
/// Представляє додаткову послугу конференц-залу.
/// </summary>
public class RoomOption
{
    /// <summary>
    /// Унікальний ідентифікатор послуги.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ідентифікатор конференц-залу.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Конференц-зал, якому належить послуга.
    /// </summary>
    public Room Room { get; set; } = null!;

    /// <summary>
    /// Назва додаткової послуги.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Вартість додаткової послуги.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Вказує, чи доступна послуга для бронювання.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Список використань послуги у бронюваннях.
    /// </summary>
    public ICollection<BookingOption> BookingOptions { get; set; } = new List<BookingOption>();
}
