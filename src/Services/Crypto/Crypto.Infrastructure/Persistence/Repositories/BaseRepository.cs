using System.Linq.Expressions;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Queryable.Common.Extensions;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T>(CryptoDbContext dbContext) : IRepository<T> where T : Entity
    {
        private CryptoDbContext Context { get; } = dbContext;
        protected DbSet<T> Set => Context.Set<T>();

        public async Task<List<T>> GetAll(bool track = false, CancellationToken ct = default)
        {
            return await Set.ApplyTracking(track).ToListAsync(ct);
        }
        
        public async Task<T?> Find(object id, CancellationToken ct = default)
        {
            return await Set.FindAsync([id], cancellationToken: ct);
        }

        public async Task<T?> First(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
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

        public async Task<int> Count(Expression<Func<T, bool>> predicate, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default, 
            CancellationToken ct = default)
        {
            return await Set
                .Where(predicate)
                .ApplyOrderBy(orderBy)
                .CountAsync(ct);
        }

        public async Task<PageResult<T>> Page(Expression<Func<T, bool>> predicate,
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
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageResult<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageResult<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates,
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
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageResult<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageResult<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates,
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
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(track)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageResult<T>(totalCount, pageQuery.Page, items);
        }
    }
}
