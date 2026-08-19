using MeetingRoom.Domain.Entities;
using MeetingRoom.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoom.Infrastructure.Repositories
{
    public class RoomBookingRepository : BaseRepository<RoomBooking>, IRoomBookingRepository
    {
        /// <summary>
        /// Ініціалізує репозиторій бронювань.
        /// </summary>
        /// <param name="context">Контекст бази даних.</param>
        public RoomBookingRepository(RoomDbContext context)
            : base(context) { }

        /// <summary>
        /// Повертає бронювання, що перетинаються із заданим періодом.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Список бронювань за вказаний період.</returns>
        public async Task<List<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .Where(booking => booking.StartTime < to && booking.EndTime > from)
                .ToListAsync();
        }

        /// <summary>
        /// Повертає активні бронювання, що перетинаються із заданим періодом.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Список активних бронювань за вказаний період.</returns>
        public async Task<List<RoomBooking>> GetActiveByPeriodAsync(
            DateTime from,
            DateTime to
        )
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .Where(booking =>
                    booking.Status == BookingStatus.Active
                    && booking.StartTime < to
                    && booking.EndTime > from
                )
                .ToListAsync();
        }

        /// <summary>
        /// Перевіряє наявність активного бронювання, що перетинається із заданим часом.
        /// </summary>
        /// <param name="roomId">Ідентифікатор конференц-залу.</param>
        /// <param name="startTime">Час початку нового бронювання.</param>
        /// <param name="endTime">Час завершення нового бронювання.</param>
        /// <returns>True, якщо знайдено конфлікт бронювання; інакше false.</returns>
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

        /// <summary>
        /// Повертає всі бронювання разом із залами та вибраними послугами.
        /// </summary>
        /// <returns>Список усіх бронювань.</returns>
        public override async Task<List<RoomBooking>> GetAllAsync()
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .ToListAsync();
        }

        /// <summary>
        /// Повертає бронювання за його ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор бронювання.</param>
        /// <returns>Знайдене бронювання або null.</returns>
        public override async Task<RoomBooking?> GetByIdAsync(Guid id)
        {
            return await _context
                .RoomBookings.Include(booking => booking.Room)
                .Include(booking => booking.SelectedOptions)
                .FirstOrDefaultAsync(booking => booking.Id == id);
        }
    }
}
