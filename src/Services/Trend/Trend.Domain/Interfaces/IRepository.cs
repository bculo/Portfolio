using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Domain.Interfaces
{
    public interface IRepository<T> where T : IDocument
    {
        Task Add(T entity);
        Task Add(ICollection<T> entities);
        Task Delete(string id);
        Task<T> FindById(string id);
        Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression);
        Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, int page, int take);
        Task<List<T>> GetAll();
    }
}
