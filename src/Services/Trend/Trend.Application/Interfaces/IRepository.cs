using System.Linq.Expressions;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Domain.Entities;

namespace Trend.Application.Interfaces
{
    public interface IRepository<T> where T : RootDocument
    {
        Task Add(T entity, CancellationToken token = default);
        IAsyncEnumerable<T> GetAllEnumerable(CancellationToken token = default);
        Task Add(ICollection<T> entities, CancellationToken token = default);
        Task Delete(string id, CancellationToken token = default);
        Task Update(T updatedEntity, CancellationToken token = default);
        Task<T?> FindById(string id, CancellationToken token = default);
        Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, CancellationToken token = default);
        Task<List<T>> GetAll(CancellationToken token = default);
        Task<long> Count(CancellationToken token = default);
    }
}
