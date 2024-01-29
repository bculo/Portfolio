using Microsoft.VisualBasic.FileIO;
using Stock.Core.Entities;
using System.Linq.Expressions;

namespace Stock.Application.Interfaces.Persistence
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Find(object id);
        Task<T> First(Expression<Func<T, bool>> filter);
        Task Add(params T[] entities);
        Task<List<T>> Filter(Expression<Func<T, bool>> filter);
        Task<(int count, List<T> page)> Page(Expression<Func<T, bool>> filter, int page, int take);
        Task<List<TType>> Filter<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> select) where TType : class;        
        Task<Dictionary<KeyType, ValueT>> GetDictionary<KeyType, ValueT>(Expression<Func<T, bool>> filter, Func<T, KeyType> key, Func<T, ValueT> elementSelector);


        Task SaveChanges();
    }
}
