using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Queryable.Common;
using Queryable.Common.Extensions;
using Queryable.Common.Models;
using Stock.Core.Models.Base;
using Stock.Core.Models.Common;

namespace Stock.Application.Interfaces.Repositories;

public interface IBaseReadRepository<T> where T : class, IReadModel
{
    Task<List<T>> GetAll(CancellationToken ct = default);
    
    Task<T?> First(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
    
    Task<List<T>> Filter(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        bool splitQuery = false,
        CancellationToken ct = default);

    Task<PageModel<T>> PageDynamic(List<QueryFilter> filters,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
    
    Task<PageModel<T>> Page(Expression<Func<T, bool>> predicates,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
        
    Task<PageModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
    
    Task<PageModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
        SortBy sortBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
        
    Task<PageModel<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
}