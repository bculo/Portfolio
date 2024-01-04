using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class ArticleRepository : MongoAuditableRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IMongoClient client, 
            IOptions<MongoOptions> options,
            IDateTimeProvider provider) 
            : base(client, options, provider)
        {
            
        }
        
        public Task<List<Article>> GetActiveArticles(ContextType type, CancellationToken token)
        {
            var result = _collection.Find(i => i.IsActive && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();

            return Task.FromResult(result);
        }

        public async IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type, 
            [EnumeratorCancellation] CancellationToken token)
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
    }
}
