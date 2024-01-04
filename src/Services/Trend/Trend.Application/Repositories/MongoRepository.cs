using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Utils;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;
using Trend.Domain.Queries.Responses.Common;

namespace Trend.Application.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : RootDocument
    {
        protected readonly MongoOptions _options;
        protected IMongoCollection<T> _collection;
        protected IDateTimeProvider _timeProvider;

        public MongoRepository(IMongoClient client, IOptions<MongoOptions> options, IDateTimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _options = options.Value;
            var database = client.GetDatabase(_options.DatabaseName);
            _collection = database.GetCollection<T>(TrendMongoUtils.GetCollectionName(typeof(T).Name));
        }

        protected virtual IQueryable<T> GetQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task Add(T entity, CancellationToken token)
        {
            entity.Created = _timeProvider.Now;
            await _collection.InsertOneAsync(entity, new InsertOneOptions(), token);
        }

        public virtual async Task Add(ICollection<T> entities, CancellationToken token)
        {
            var date = _timeProvider.Now;
            foreach (var entity in entities)
            {
                entity.Created = date;
            }
            
            await _collection.InsertManyAsync(entities, new InsertManyOptions(), token);
        }

        public virtual async Task Delete(string id, CancellationToken token)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            await _collection.DeleteOneAsync(filter, token);
        }

        public virtual Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, CancellationToken token)
        {
            return Task.FromResult(_collection.Find(filterExpression).SortByDescending(i => i.Created).ToList());
        }

        public virtual Task<PageResponse<T>> FilterBy(int page, int take, Expression<Func<T, bool>> filterExpression = null, CancellationToken token = default)
        {
            filterExpression ??= i => true;

            var count = _collection.CountDocuments(filterExpression);
            var items = _collection.Find(filterExpression)
                .SortByDescending(i => i.Created)
                .Skip((page - 1) * take)
                .Limit(take)
                .ToList();

            return Task.FromResult(new PageResponse<T>(count, items));
        }

        public virtual async Task<T> FindById(string id, CancellationToken token)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            var result = await _collection.FindAsync(filter, new FindOptions<T>(), token);
            return result.FirstOrDefault();
        }

        public virtual Task<List<T>> GetAll(CancellationToken token)
        {
            return Task.FromResult(GetQueryable().ToList());
        }

        public virtual async Task<long> Count(CancellationToken token)
        {
            return await _collection.CountDocumentsAsync(i => true, new CountOptions(), token);
        }

        public virtual async IAsyncEnumerable<T> GetAllEnumerable([EnumeratorCancellation] CancellationToken token)
        {
            using var cursor = await _collection.Find(i => true).ToCursorAsync(token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var current in cursor.Current)
                {
                    yield return current;
                }
            }
        }
    }
}
