using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Base;
using Stock.Core.Models.Common;
using Stock.Infrastructure.Persistence.Extensions;

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
        
        public async Task<PageReadModel<T>> Page(Expression<Func<T, bool>> predicate,
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
                .ApplyPagination(pageQuery)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageReadModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageReadModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates,
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
                .ApplyPagination(pageQuery)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);
            
            return new PageReadModel<T>(totalCount, pageQuery.Page, items);
        }

        public async Task<PageReadModel<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates,
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
                .ApplyPagination(pageQuery)
                .ApplyTracking(false)
                .ApplySplitQuery(splitQuery)
                .ToListAsync(ct);

            return new PageReadModel<T>(totalCount, pageQuery.Page, items);
        }
}