namespace Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Entities;
internal sealed class RoomEntity
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public decimal PricePerHour { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<RoomOptionEntity> Options { get; } = new();
}
