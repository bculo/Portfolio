using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Queries.Requests.Common;
using Trend.Domain.Queries.Responses.Common;

namespace Trend.Domain.Interfaces
{
    public interface IRepository<T> where T : IDocumentRoot
    {
        Task Add(T entity);
        IAsyncEnumerable<T> GetAllEnumerable();
        Task Add(ICollection<T> entities);
        Task Delete(string id);
        Task<T> FindById(string id);
        Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression);
        Task<PageResponse<T>> FilterBy(int page, int take, Expression<Func<T, bool>> filterExpression = null);
        Task<List<T>> GetAll();
        Task<long> Count();
    }
}
