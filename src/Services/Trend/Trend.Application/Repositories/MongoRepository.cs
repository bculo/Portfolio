using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Options;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IDocument
    {
        protected readonly MongoOptions _options;
        protected IMongoCollection<T> _collection;

        public MongoRepository(IOptions<MongoOptions> options)
        {
            _options = options.Value;

            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task Add(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task Add(ICollection<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public virtual async Task Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        public virtual async Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).SortByDescending(i => i.Created).ToList();
        }

        public virtual async Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, int page, int take)
        {
            return _collection.Find(filterExpression)
                .SortByDescending(i => i.Created)
                .Skip((page - 1) * take)
                .Limit(take)
                .ToList();
        }

        public virtual async Task<T> FindById(string id)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            var result = await _collection.FindAsync(filter);
            return result.FirstOrDefault();
        }

        public virtual async Task<List<T>> GetAll()
        {
            return GetQueryable().ToList();
        }
    }
}
