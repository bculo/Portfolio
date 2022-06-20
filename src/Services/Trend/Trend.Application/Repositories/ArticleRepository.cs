using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Options;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;
using Trend.Domain.Queries.Requests.Common;
using Trend.Domain.Queries.Responses.Common;

namespace Trend.Application.Repositories
{
    public class ArticleRepository : MongoRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IOptions<MongoOptions> options) : base(options)
        {

        }

        public async Task<List<Article>> GetActiveArticles()
        {
            return _collection.Find(i => i.IsActive)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public async IAsyncEnumerable<Article> GetActiveArticlesEnumerable()
        {
            using (var cursor = await _collection.Find(i => i.IsActive)
                .SortByDescending(i => i.Created)
                .ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var item in cursor.Current)
                    {
                        yield return item;
                    }
                }
            }
        }

        public async Task<List<Article>> GetActiveArticles(ContextType type)
        {
            return _collection.Find(i => i.IsActive && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public async IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type)
        {
            using (var cursor = await _collection.Find(i => i.IsActive && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var item in cursor.Current)
                    {
                        yield return item;
                    }
                }
            }
        }

        public async Task<List<Article>> GetArticles(DateTime from, DateTime to, ContextType type)
        {
            return _collection.Find(i => i.Created >= from && i.Created <= to && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public async IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to, ContextType type)
        {
            using (var cursor = await _collection.Find(i => i.Created >= from && i.Created <= to && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var item in cursor.Current)
                    {
                        yield return item;
                    }
                }
            }
        }

        public async Task<List<Article>> GetArticles(DateTime from, DateTime to)
        {
            return _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public async IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to)
        {
            using (var cursor = await _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var item in cursor.Current)
                    {
                        yield return item;
                    }
                }
            }
        }

        public async Task DeactivateArticles(List<string> articleIds)
        {
            var update = Builders<Article>.Update.Set(s => s.IsActive, false);
            await _collection.UpdateManyAsync(i => articleIds.Contains(i.Id), update);
        }

        public async Task<PageResponse<Article>> GetPageFilter(PageRequest<ContextType> searchRepo)
        {
            throw new NotImplementedException();
        }
    }
}
