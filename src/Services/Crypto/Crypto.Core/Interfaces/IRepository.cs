using Crypto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> GetAll();
        Task<List<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T?> FindSingle(Expression<Func<T, bool>> predicate);
        Task<List<T>> FetchPage(int page, int take);
        Task<List<T>> FetchPage(Expression<Func<T, bool>> predicate, int page, int take);
        Task<T?> FindById(object id);
        Task Add(T newInstance);
        Task AddRange(IEnumerable<T> instances);
        Task Remove(T instance);
        Task<long> Count();
    }
}
