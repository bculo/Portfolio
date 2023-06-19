using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly StockDbContext _dbContext;

        public BaseRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual Task Add(params T[] entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            return Task.CompletedTask;
        }

        public virtual async Task<List<T>> Filter(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().Where(filter).ToListAsync();
        }

        public virtual async Task<List<TType>> Filter<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbContext.Set<T>().Where(filter).Select(select).ToListAsync();
        }

        public virtual async Task<T> Find(object id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> First(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().AsTracking().FirstOrDefaultAsync(filter);
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<Dictionary<KeyType, ValueT>> GetDictionary<KeyType, ValueT>(Expression<Func<T, bool>> filter, 
            Func<T, KeyType> key, 
            Func<T, ValueT> elementSelector)
        {
            return await _dbContext.Set<T>().Where(filter).ToDictionaryAsync(key, elementSelector);
        }

        public virtual async Task<(int count, List<T> page)> Page(Expression<Func<T, bool>> filter, int page, int take)
        {
            var query = _dbContext.Set<T>().Where(filter);
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * take).Take(take).ToListAsync();
            return (count, items);
        }

        public virtual async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
