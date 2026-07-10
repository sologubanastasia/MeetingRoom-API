using MeetingRoom.Domain.Entities;
using MeetingRoom.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoom.Infrastructure.Repositories
{
    public class RoomBookingRepository : BaseRepository<RoomBooking>, IRoomBookingRepository
    {
        public RoomBookingRepository(RoomDbContext context)
            : base(context) { }

        public async Task<List<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .Where(booking => booking.StartTime < to && booking.EndTime > from)
                .ToListAsync();
        }

        public async Task<bool> HasTimeConflictAsync(
            Guid roomId,
            DateTime startTime,
            DateTime endTime
        )
        {
            return await _context.RoomBookings.AnyAsync(booking =>
                booking.RoomId == roomId
                && booking.Status == BookingStatus.Active
                && booking.StartTime < endTime
                && startTime < booking.EndTime
            );
        }

        public new async Task<List<RoomBooking>> GetAllAsync()
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .ToListAsync();
        }

        public new async Task<RoomBooking?> GetByIdAsync(Guid id)
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .FirstOrDefaultAsync(booking => booking.Id == id);
        }
    }
}
