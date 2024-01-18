using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Application.Utils;
using Trend.Domain.Entities;

namespace Trend.Application.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : RootDocument
    {
        private readonly IMongoDatabase _mongoDatabase;
        public IDateTimeProvider TimeProvider { get; }
        public IMongoCollection<T> Collection { get; }
        public IClientSessionHandle ClientSession { get; }

        
        public MongoRepository(IMongoClient client, 
            IOptions<MongoOptions> options, 
            IDateTimeProvider timeProvider,
            IClientSessionHandle clientSession,
            IMongoDatabase mongoDatabase)
        {
            ClientSession = clientSession;
            TimeProvider = timeProvider;
            _mongoDatabase = mongoDatabase;
            Collection = _mongoDatabase.GetCollection<T>(TrendMongoUtils.GetCollectionName(typeof(T).Name));
        }

        protected IQueryable<T> GetQueryable()
        {
            return Collection.AsQueryable();
        }

        
        public virtual async Task Add(T entity, CancellationToken token = default)
        {
            entity.Created = TimeProvider.Now;
            await Collection.InsertOneAsync(ClientSession, entity, new InsertOneOptions(), token);
        }

        public virtual async Task Add(ICollection<T> entities, CancellationToken token = default)
        {
            var date = TimeProvider.Now;
            foreach (var entity in entities)
            {
                entity.Created = date;
            }
            await Collection.InsertManyAsync(ClientSession, entities, new InsertManyOptions(), token);
        }

        public virtual async Task Delete(string id, CancellationToken token = default)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            await Collection.DeleteOneAsync(ClientSession, filter, new DeleteOptions(), token);
        }

        public virtual async Task<List<T>> FilterBy(Expression<Func<T, bool>> filterExpression, CancellationToken token = default)
        {
            return await Collection.Find(ClientSession, filterExpression)
                .SortByDescending(i => i.Created)
                .ToListAsync(token);
        }
        
        public virtual async Task<T?> FindById(string id, CancellationToken token = default)
        {
            var filter = Builders<T>.Filter.Eq(t => t.Id, id);
            var result = await Collection.FindAsync(ClientSession, filter, new FindOptions<T>(), token);
            return result.FirstOrDefault();
        }

        public virtual Task<List<T>> GetAll(CancellationToken token = default)
        {
            return Collection.Find(ClientSession, i => true).ToListAsync(token);
        }
        
        public virtual async Task Update(T updatedEntity, CancellationToken token = default)
        {
            await Collection.ReplaceOneAsync(ClientSession, x => x.Id == updatedEntity.Id, 
                updatedEntity, 
                new ReplaceOptions(),
                token);
        }

        public virtual async Task<long> Count(CancellationToken token = default)
        {
            return await Collection.CountDocumentsAsync(ClientSession, i => true, new CountOptions(), token);
        }

        public virtual async IAsyncEnumerable<T> GetAllEnumerable([EnumeratorCancellation] CancellationToken token = default)
        {
            using var cursor = await Collection.Find(ClientSession, i => true).ToCursorAsync(token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var current in cursor.Current)
                {
                    yield return current;
                }
            }
        }

        protected IMongoCollection<TCollection> GetCollection<TCollection>()
        {
            return _mongoDatabase.GetCollection<TCollection>(TrendMongoUtils.GetCollectionName(typeof(TCollection).Name));
        }
    }
}
