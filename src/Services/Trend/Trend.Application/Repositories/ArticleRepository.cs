using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trend.Application.Configurations.Options;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class ArticleRepository : MongoRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IMongoClient client, IOptions<MongoOptions> options) 
            : base(client, options)
        {
        }

        public Task<List<Article>> GetActiveArticles(CancellationToken token)
        {
            var result = _collection.Find(i => i.IsActive)
                .SortByDescending(i => i.Created)
                .ToList();

            return Task.FromResult(result);
        }

        public async IAsyncEnumerable<Article> GetActiveArticlesEnumerable([EnumeratorCancellation] CancellationToken token)
        {
            using var cursor = await _collection.Find(i => i.IsActive)
                .SortByDescending(i => i.Created)
                .ToCursorAsync(token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public Task<List<Article>> GetActiveArticles(ContextType type, CancellationToken token)
        {
            var result = _collection.Find(i => i.IsActive && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();

            return Task.FromResult(result);
        }

        public async IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type, [EnumeratorCancellation] CancellationToken token)
        {
            using var cursor = await _collection.Find(i => i.IsActive && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToCursorAsync(token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public Task<List<Article>> GetArticles(DateTime from, DateTime to, ContextType type, CancellationToken token)
        {
            var result = _collection.Find(i => i.Created >= from && i.Created <= to && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();

            return Task.FromResult(result);
        }

        public async IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to, ContextType type, [EnumeratorCancellation]CancellationToken token)
        {
            using var cursor = await _collection.Find(i => i.Created >= from && i.Created <= to && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToCursorAsync(token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public Task<List<Article>> GetArticles(DateTime from, DateTime to, CancellationToken token)
        {
            var result =  _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToList();

            return Task.FromResult(result);
        }

        public async IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to, [EnumeratorCancellation] CancellationToken token)
        {
            using var cursor = await _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToCursorAsync(token);
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public async Task DeactivateArticles(List<string> articleIds, CancellationToken token)
        {
            var update = Builders<Article>.Update.Set(s => s.IsActive, false);
            await _collection.UpdateManyAsync(i => articleIds.Contains(i.Id), update, new UpdateOptions(), token);
        }
    }
}
