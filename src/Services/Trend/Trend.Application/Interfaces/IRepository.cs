using System.Linq.Expressions;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Domain.Entities;

namespace Trend.Application.Interfaces
{
    public interface IRepository<T> where T : RootDocument
    {
        Task Add(T entity, CancellationToken token);
        IAsyncEnumerable<T> GetAllEnumerable(CancellationToken token);
        Task Add(ICollection<T> entities, CancellationToken token);
        Task Delete(string id, CancellationToken token);
        Task<T> FindById(string id, CancellationToken token);
        Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, CancellationToken token);
        Task<PageResQuery<T>> FilterBy(int page, int take, Expression<Func<T, bool>> filterExpression = null, CancellationToken token = default);
        Task<List<T>> GetAll(CancellationToken token);
        Task<long> Count(CancellationToken token);
    }
}
