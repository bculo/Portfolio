using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Stock.Core.Models.Common;

namespace Stock.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll(CancellationToken ct = default);
        
        Task<T> Find(object id, CancellationToken ct = default);
        
        Task<T?> First(Expression<Func<T, bool>> predicate, 
            CancellationToken ct = default);
        
        Task<T?> First(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            CancellationToken ct = default);
        
        Task Add(T entity, CancellationToken ct = default);
        
        Task AddRange(IEnumerable<T> entities, CancellationToken ct = default);

        Task<List<T>> Filter(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
            bool asTracking = false,
            CancellationToken ct = default);

        Task<PageReadModel<T>> Page(Expression<Func<T, bool>> predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            CancellationToken ct = default);
        
        Task<PageReadModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            CancellationToken ct = default);
        
        Task<PageReadModel<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            CancellationToken ct = default);
    }
}
