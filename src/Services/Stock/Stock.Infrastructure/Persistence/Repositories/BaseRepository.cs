using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Common;
using Stock.Infrastructure.Persistence.Extensions;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public StockDbContext Context { get; }
        public DbSet<T> Set => Context.Set<T>();

        public BaseRepository(StockDbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task<List<T>> GetAll(bool track = false, CancellationToken ct = default)
        {
            return await Set.ApplyTracking(track).ToListAsync(ct);
        }
        
        public async Task<T?> Find(object id, CancellationToken ct = default)
        {
            return await Set.FindAsync(id, ct);
        }

        public async Task<T?> First(Expression<Func<T, bool>> predicate,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            return await Set
                .Where(predicate)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .FirstOrDefaultAsync(ct);
        }
        
        public async Task<T?> First(Expression<Func<T, bool>> predicate, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            return await Set
                .Where(predicate)
                .ApplyInclude(include)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .FirstOrDefaultAsync(ct);
        }

        public async Task Add(T entity, CancellationToken ct = default)
        {
            await Set.AddAsync(entity, ct);
        }

        public async Task AddRange(IEnumerable<T> entities, CancellationToken ct = default)
        {
            await Set.AddRangeAsync(entities, ct);
        }

        public Task Remove(T entity, CancellationToken ct = default)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveAll(IEnumerable<T> entities, CancellationToken ct = default)
        {
            Set.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task RemoveAll(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            await Set.Where(predicate).ExecuteDeleteAsync(ct);
        }

        public async Task UpdateAll(Expression<Func<T, bool>> predicate, 
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setters, 
            CancellationToken ct = default)
        {
            await Set.Where(predicate).ExecuteUpdateAsync(setters, ct);
        }

        public async Task<List<T>> Filter(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default, 
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            return await Set
                .Where(predicate)
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
        }
        
        public async Task<PageModel<T>> Page(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.Where(predicate);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageQuery)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAll(predicates);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageQuery)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageModel<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAny(predicates);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageQuery)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }
    }
}
