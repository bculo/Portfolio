using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;
using Trend.Domain.Queries.Requests.Common;
using Trend.Domain.Queries.Responses.Common;

namespace Trend.Domain.Interfaces
{
    public interface IRepository<T> where T : RootDocument
    {
        Task Add(T entity, CancellationToken token);
        IAsyncEnumerable<T> GetAllEnumerable(CancellationToken token);
        Task Add(ICollection<T> entities, CancellationToken token);
        Task Delete(string id, CancellationToken token);
        Task<T> FindById(string id, CancellationToken token);
        Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, CancellationToken token);
        Task<PageResponse<T>> FilterBy(int page, int take, Expression<Func<T, bool>> filterExpression = null, CancellationToken token = default);
        Task<List<T>> GetAll(CancellationToken token);
        Task<long> Count(CancellationToken token);
    }
}
