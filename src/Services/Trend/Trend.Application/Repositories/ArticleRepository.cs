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

namespace Trend.Application.Repositories
{
    public class ArticleRepository : MongoRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IOptions<MongoOptions> options) : base(options)
        {

        }

        public async Task<List<Article>> GetArticles(DateTime from, DateTime to, ContextType type)
        {
            return _collection.Find(i => i.Created >= from && i.Created <= to && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public async Task<List<Article>> GetArticles(DateTime from, DateTime to)
        {
            return _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public async IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to)
        {
            using(var cursor = await _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach(var item in cursor.Current)
                    {
                        yield return item;
                    }
                }
            }
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
    }
}
