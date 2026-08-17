using AutoMapper;
using MeetingRoom.Application.Dtos.Rooms;
using MeetingRoom.Domain.Entities;
using MeetingRoom.Domain.Interfaces;

namespace MeetingRoom.Application.Services.Rooms
{
    /// <summary>
    /// Реалізує бізнес-логіку управління конференц-залами.
    /// </summary>
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<List<RoomResponse>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();

            return rooms
                .Where(room => !room.IsDeleted)
                .Select(room => _mapper.Map<RoomResponse>(room))
                .ToList();
        }

        public async Task<RoomResponse?> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null || room.IsDeleted)
            {
                return null;
            }

            return _mapper.Map<RoomResponse>(room);
        }

        public async Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Capacity = request.Capacity,
                PricePerHour = request.PricePerHour,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,

                Options = request
                    .Options.Select(option => new RoomOption
                    {
                        Id = Guid.NewGuid(),
                        Name = option.Name,
                        Price = option.Price,
                        IsActive = true,
                    })
                    .ToList(),
            };

            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();

            return _mapper.Map<RoomResponse>(room);
        }

        public async Task<RoomResponse?> UpdateRoomAsync(Guid id, UpdateRoomRequest request)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null || room.IsDeleted)
            {
                return null;
            }

            room.Name = request.Name;
            room.Capacity = request.Capacity;
            room.PricePerHour = request.PricePerHour;

            foreach (var existingOption in room.Options)
            {
                existingOption.IsActive = false;
            }

            foreach (var option in request.Options)
            {
                room.Options.Add(
                    new RoomOption
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        Name = option.Name,
                        Price = option.Price,
                        IsActive = true,
                    }
                );
            }

            _roomRepository.Update(room);
            await _roomRepository.SaveChangesAsync();

            return _mapper.Map<RoomResponse>(room);
        }

        public async Task<bool> DeleteRoomAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null || room.IsDeleted)
            {
                return false;
            }

            room.IsDeleted = true;

            _roomRepository.Update(room);
            await _roomRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<RoomResponse>> GetAvailableRoomsAsync(
            DateTime startTime,
            DateTime endTime,
            int capacity
        )
        {
            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.");
            }

            var rooms = await _roomRepository.GetAvailableAsync(startTime, endTime, capacity);

            return rooms.Select(room => _mapper.Map<RoomResponse>(room)).ToList();
        }
    }
}
