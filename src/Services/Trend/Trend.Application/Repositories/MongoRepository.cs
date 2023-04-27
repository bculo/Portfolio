using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Trend.Application.Options;
using Trend.Domain.Interfaces;
using Trend.Domain.Queries.Responses.Common;

namespace Trend.Application.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IDocumentRoot
    {
        protected readonly IMongoClient _client;
        protected readonly MongoOptions _options;
        protected IMongoCollection<T> _collection;

        public MongoRepository(IMongoClient client, IOptions<MongoOptions> options)
        {
            _client = client;
            _options = options.Value;
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

        public virtual async Task<PageResponse<T>> FilterBy(int page, int take, Expression<Func<T, bool>> filterExpression = null)
        {
            if(filterExpression is null)
            {
                filterExpression = i => true;
            }

            var count = _collection.CountDocuments(filterExpression);
            var items = _collection.Find(filterExpression)
                .SortByDescending(i => i.Created)
                .Skip((page - 1) * take)
                .Limit(take)
                .ToList();

            return new PageResponse<T>(count, items);
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

        public virtual async Task<long> Count()
        {
            return _collection.CountDocuments(i => true);
        }

        public virtual async IAsyncEnumerable<T> GetAllEnumerable()
        {
            using(var cursor = await _collection.Find(i => true).ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var current in cursor.Current)
                    {
                        yield return current;
                    }
                }
            }
        }
    }
}
