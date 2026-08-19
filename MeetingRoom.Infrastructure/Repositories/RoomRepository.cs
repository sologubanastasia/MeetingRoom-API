using MeetingRoom.Domain.Entities;
using MeetingRoom.Domain.Interfaces;
using MeetingRoom.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoom.Infrastructure.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(RoomDbContext context)
            : base(context) { }

        public override async Task<List<Room>> GetAllAsync()
        {
            return await _context
                .Rooms.Include(room => room.Options)
                .Where(room => !room.IsDeleted)
                .ToListAsync();
        }

        public override async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _context
                .Rooms.Include(room => room.Options)
                .Include(room => room.Bookings)
                .FirstOrDefaultAsync(room => room.Id == id && !room.IsDeleted);
        }

        public async Task<List<Room>> GetAvailableAsync(
            DateTime startTime,
            DateTime endTime,
            int capacity
        )
        {
            return await _context
                .Rooms.Include(room => room.Options)
                .Include(room => room.Bookings)
                .Where(room =>
                    !room.IsDeleted
                    && room.Capacity >= capacity
                    && !room.Bookings.Any(booking =>
                        booking.Status == BookingStatus.Active
                        && booking.StartTime < endTime
                        && startTime < booking.EndTime
                    )
                )
                .ToListAsync();
        }
    }
}
