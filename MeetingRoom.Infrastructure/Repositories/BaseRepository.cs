using MeetingRoom.Domain.Interfaces;
using MeetingRoom.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoom.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        protected readonly RoomDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(RoomDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
