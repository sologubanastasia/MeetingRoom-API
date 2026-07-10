using MeetingRoom.Domain.Entities;

namespace MeetingRoom.Infrastructure
{
    public static class SeedData
    {
        public static readonly Guid RoomAId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public static readonly Guid RoomBId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        public static readonly Guid RoomCId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public static readonly DateTime CreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly Room[] Rooms =
        {
            new()
            {
                Id = RoomAId,
                Name = "Hall A",
                Capacity = 50,
                PricePerHour = 2000m,
                IsDeleted = false,
                CreatedAt = CreatedAt,
            },
            new()
            {
                Id = RoomBId,
                Name = "Hall B",
                Capacity = 100,
                PricePerHour = 3500m,
                IsDeleted = false,
                CreatedAt = CreatedAt,
            },
            new()
            {
                Id = RoomCId,
                Name = "Hall C",
                Capacity = 30,
                PricePerHour = 1500m,
                IsDeleted = false,
                CreatedAt = CreatedAt,
            },
        };

        public static readonly RoomOption[] RoomOptions =
        {
            CreateOption("00000000-0000-0000-0000-000000000101", RoomAId, "Projector", 500m),
            CreateOption("00000000-0000-0000-0000-000000000102", RoomAId, "Wi-Fi", 300m),
            CreateOption("00000000-0000-0000-0000-000000000103", RoomAId, "Sound", 700m),
            CreateOption("00000000-0000-0000-0000-000000000201", RoomBId, "Projector", 500m),
            CreateOption("00000000-0000-0000-0000-000000000202", RoomBId, "Wi-Fi", 300m),
            CreateOption("00000000-0000-0000-0000-000000000203", RoomBId, "Sound", 700m),
            CreateOption("00000000-0000-0000-0000-000000000301", RoomCId, "Projector", 500m),
            CreateOption("00000000-0000-0000-0000-000000000302", RoomCId, "Wi-Fi", 300m),
            CreateOption("00000000-0000-0000-0000-000000000303", RoomCId, "Sound", 700m),
        };

        private static RoomOption CreateOption(string id, Guid roomId, string name, decimal price)
        {
            return new RoomOption
            {
                Id = Guid.Parse(id),
                RoomId = roomId,
                Name = name,
                Price = price,
                IsActive = true,
            };
        }
    }
}
