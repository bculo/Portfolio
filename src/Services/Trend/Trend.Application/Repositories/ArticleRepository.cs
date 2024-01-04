using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Application.Repositories.Lookups;
using Trend.Application.Repositories.Unwinds;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

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
        
        public async Task<List<ArticleDetailResQuery>> GetActiveArticles(ContextType type, CancellationToken token)
        {
            var wordCollection = GetCollection<SearchWord>();
            var result = await _collection.Aggregate()
                .Match(x => x.IsActive == true)
                .Lookup<Article, SearchWord, ArticleSearchWordLookup>(wordCollection,
                    x => x.SearchWordId,
                    y => y.Id,
                    y => y.SearchWords)
                .Unwind<ArticleSearchWordLookup, ArticleSearchWordUnwind>(x => x.SearchWords)
                .Project(x => new ArticleDetailResQuery
                {
                    Id = x.Id,
                    SearchWord = x.SearchWords.Word,
                    ContextType = x.SearchWords.Type,
                    Content = x.Content,
                    SearchWordId = x.SearchWordId,
                    Text = x.Text,
                    Created = x.Created,
                    PageSource = x.PageSource,
                    Title = x.Title,
                    SearchWordImage = x.SearchWords.ImageUrl,
                    ArticleUrl = x.ArticleUrl
                })
                .ToListAsync(token);

            return result;
        }

        public async IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type, 
            [EnumeratorCancellation] CancellationToken token)
        {
            yield return null;
            /*
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
            */
        }
    }
}
