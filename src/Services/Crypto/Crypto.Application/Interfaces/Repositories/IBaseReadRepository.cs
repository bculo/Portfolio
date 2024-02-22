using System.Linq.Expressions;
using Crypto.Application.Interfaces.Repositories.Models;
using Microsoft.EntityFrameworkCore.Query;
using Queryable.Common.Extensions;

namespace Crypto.Application.Interfaces.Repositories;

public interface IBaseReadRepository<T> where T : class
{
    Task<List<T>> GetAll(CancellationToken ct = default);
    
    Task<T?> First(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
    
    Task<List<T>> Filter(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        bool splitQuery = false,
        CancellationToken ct = default);

    Task<PageResult<T>> PageDynamic(List<QueryFilter> filters,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
    
    Task<PageResult<T>> Page(Expression<Func<T, bool>> predicates,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
        
    Task<PageResult<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
        
    Task<PageResult<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        PageQuery pageQuery,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        bool splitQuery = false,
        CancellationToken ct = default);
}