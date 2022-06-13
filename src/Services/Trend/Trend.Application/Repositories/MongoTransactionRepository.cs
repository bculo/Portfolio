using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Repositories;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class MongoTransactionRepository<T> : IRepository<T> where T : IDocument
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<T> _dbSet;

        public MongoTransactionRepository(IMongoContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public virtual async Task Add(T entity)
        {
            _context.AddCommand(async () => await _dbSet.InsertOneAsync(entity));
        }

        public virtual async Task Add(ICollection<T> entities)
        {
            _context.AddCommand(async () => await _dbSet.InsertManyAsync(entities));
        }

        public virtual async Task Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            _context.AddCommand(async () => await _dbSet.DeleteOneAsync(filter));
        }

        public virtual async Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression)
        {
            return _dbSet.Find(filterExpression).SortByDescending(i => i.Created).ToList();
        }

        public virtual async Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, int page, int take)
        {
            return _dbSet.Find(filterExpression)
                .SortByDescending(i => i.Created)
                .Skip((page - 1) * take)
                .Limit(take)
                .ToList();
        }

        public virtual async Task<T> FindById(string id)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            var result = await _dbSet.FindAsync(filter);
            return result.FirstOrDefault();
        }

        public virtual async Task<List<T>> GetAll()
        {
            return GetQueryable().ToList();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
