using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Queryable.Common.Extensions;
using Queryable.Common.Models;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Base;
using Stock.Core.Models.Common;

namespace Stock.Infrastructure.Persistence.Repositories.Read;

public class BaseReadRepository<T> : IBaseReadRepository<T> where T : class, IReadModel
{
    public StockDbContext Context { get; }
    public DbSet<T> Set => Context.Set<T>();
    
    
    public BaseReadRepository(StockDbContext context)
    {
        Context = context;
    }
    
    public async Task<List<T>> GetAll(CancellationToken ct = default)
    {
        return await Set.ToListAsync(ct);
    }

    public async Task<T?> First(Expression<Func<T, bool>> predicate, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false, 
        CancellationToken ct = default)
    {
        return await Set
            .Where(predicate)
            .ApplyOrderBy(orderBy)
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
    
    public async Task<PageModel<T>> PageDynamic(List<QueryFilter> filters, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, 
        PageQuery pageQuery, 
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default, 
        bool splitQuery = false,
        CancellationToken ct = default)
    {
        var query = Set.Where(filters);
            
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .ApplyInclude(include)
            .ApplyOrderBy(orderBy)
            .ApplyPagination(pageQuery.Skip, pageQuery.Take)
            .ApplyTracking(false)
            .ApplySplitQuery(splitQuery)
            .ToListAsync(ct);

        return new PageModel<T>(totalCount, pageQuery.Page, items);
    }

    public async Task<PageModel<T>> Page(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.Where(predicate);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAll(predicates);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
            SortBy sortBy, 
            PageQuery pageQuery, 
            Func<IQueryable<T>, 
            IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false, CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAll(predicates);
            
            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(sortBy)
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageModel<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool splitQuery = false,
            CancellationToken ct = default)
        {
            var query = Set.ApplyWhereAny(predicates);

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .ApplyInclude(include)
                .ApplyOrderBy(orderBy)
                .ApplyPagination(pageQuery.Skip, pageQuery.Take)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageModel<T>(totalCount, pageQuery.Page, items);
        }
}