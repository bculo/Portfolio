using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly StockDbContext _dbContext;

        public BaseRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Add(params T[] entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<T>> Filter(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<List<TType>> Filter<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbContext.Set<T>().Where(filter).Select(select).ToListAsync();
        }

        public async Task<T> Find(object id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> First(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().AsTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<Dictionary<KeyType, ValueT>> GetDictionary<KeyType, ValueT>(Expression<Func<T, bool>> filter, 
            Func<T, KeyType> key, 
            Func<T, ValueT> elementSelector)
        {
            return await _dbContext.Set<T>().Where(filter).ToDictionaryAsync(key, elementSelector);
        }

        public async Task<(int count, List<T> page)> Page(Expression<Func<T, bool>> filter, int page, int take)
        {
            var query = _dbContext.Set<T>().Where(filter);
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * take).Take(take).ToListAsync();
            return (count, items);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
