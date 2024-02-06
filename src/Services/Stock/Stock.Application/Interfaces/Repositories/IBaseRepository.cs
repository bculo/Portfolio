using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Stock.Core.Models.Common;

namespace Stock.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll(bool track = false, CancellationToken ct = default);
        
        Task<T?> Find(object id, CancellationToken ct = default);
        
        Task<T?> First(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
        
        Task Add(T entity, CancellationToken ct = default);
        
        Task AddRange(IEnumerable<T> entities, CancellationToken ct = default);

        Task Remove(T entity, CancellationToken ct = default);

        Task RemoveAll(IEnumerable<T> entities, CancellationToken ct = default);
        
        Task RemoveAll(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

        Task UpdateAll(Expression<Func<T, bool>> predicate,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setters,
            CancellationToken ct = default);

        Task<List<T>> Filter(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);

        Task<PageModel<T>> Page(Expression<Func<T, bool>> predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
        
        Task<PageModel<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
        
        Task<PageModel<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageQuery pageQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
    }
}
