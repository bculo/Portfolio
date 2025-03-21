using System.Linq.Expressions;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Queryable.Common.Extensions;

namespace Crypto.Infrastructure.Persistence.Repositories.Read;

public class BaseReadRepository<T>(CryptoDbContext context) : IBaseReadRepository<T>
    where T : class
{
    public CryptoDbContext Context { get; } = context;
    public DbSet<T> Set => Context.Set<T>();


    public async Task<List<T>> GetAll(CancellationToken ct = default)
    {
        return await Set.ToListAsync(ct);
    }

    public async Task<T?> First(Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false, 
        CancellationToken ct = default)
    {
        return await Set
            .Where(predicate)
            .ApplyInclude(include)
            .ApplyTracking(false)
            .ApplySplitQuery(splitQuery)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<T>> Filter(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            return await Set
                .Where(predicate)
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
        }
    
    public async Task<PageResult<T>> PageDynamic(List<QueryFilter> filters, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, 
        PageRepoQuery pageRepoQuery, 
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default, 
        bool splitQuery = false,
        CancellationToken ct = default)
    {
        var query = Set.Where(filters);
            
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .ApplyInclude(include)
            .ApplyOrderBy(orderBy)
            .ApplyPagination(pageRepoQuery.Skip, pageRepoQuery.Take)
            .ApplyTracking(false)
            .ApplySplitQuery(splitQuery)
            .ToListAsync(ct);

        return new PageResult<T>(totalCount, pageRepoQuery.Page, items);
    }

    public async Task<PageResult<T>> Page(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageRepoQuery pageRepoQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.Where(predicate);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageRepoQuery.Skip, pageRepoQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageResult<T>(totalCount, pageRepoQuery.Page, items);
        }

        public async Task<PageResult<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageRepoQuery pageRepoQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAll(predicates);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageRepoQuery.Skip, pageRepoQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageResult<T>(totalCount, pageRepoQuery.Page, items);
        }

        public async Task<PageResult<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageRepoQuery pageRepoQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAny(predicates);

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageRepoQuery.Skip, pageRepoQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageResult<T>(totalCount, pageRepoQuery.Page, items);
        }
}