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

        public virtual async Task<List<Article>> GetArticles(DateTime from, DateTime to, ArticleType type)
        {
            return _collection.Find(i => i.Created >= from && i.Created <= to && i.Type == type)
                .SortByDescending(i => i.Created)
                .ToList();
        }

        public virtual async Task<List<Article>> GetArticles(DateTime from, DateTime to)
        {
            return _collection.Find(i => i.Created >= from && i.Created <= to)
                .SortByDescending(i => i.Created)
                .ToList();
        }
    }
}
