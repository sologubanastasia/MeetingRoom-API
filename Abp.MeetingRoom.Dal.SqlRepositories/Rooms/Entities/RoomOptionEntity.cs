namespace Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Entities;
internal sealed class RoomOptionEntity
{
    public Guid Id { get; init; }
    public Guid RoomId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
}
