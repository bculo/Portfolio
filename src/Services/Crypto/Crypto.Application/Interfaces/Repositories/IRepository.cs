using System.Linq.Expressions;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Crypto.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
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
        
        Task<int> Count(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
            CancellationToken ct = default);

        Task<PageResult<T>> Page(Expression<Func<T, bool>> predicates,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageRepoQuery pageRepoQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
        
        Task<PageResult<T>> PageMatchAll(Expression<Func<T, bool>>[] predicates, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageRepoQuery pageRepoQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
        
        Task<PageResult<T>> PageMatchAny(Expression<Func<T, bool>>[] predicates, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            PageRepoQuery pageRepoQuery,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
            bool track = false,
            bool splitQuery = false,
            CancellationToken ct = default);
    }
}
