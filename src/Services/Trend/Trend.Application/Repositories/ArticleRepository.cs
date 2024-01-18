using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
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
            IDateTimeProvider provider,
            IClientSessionHandle clientSession) 
            : base(client, options, provider, clientSession)
        {
            
        }

        private IAggregateFluent<ArticleDetailResQuery> BuildAggregateBasedOnContext(ContextType type)
        {
            var wordCollection = GetCollection<SearchWord>();
            
            var aggregate = Collection.Aggregate(ClientSession)
                .Match(x => x.IsActive == true)
                .Lookup<Article, SearchWord, ArticleSearchWordLookup>(wordCollection,
                    x => x.SearchWordId,
                    y => y.Id,
                    y => y.SearchWords)
                .Unwind<ArticleSearchWordLookup, ArticleSearchWordUnwind>(x => x.SearchWords)
                .Match(x => x.SearchWords.Type.Id == type.Id)
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
                });
            
            return aggregate;
        }
        
        public async Task<List<ArticleDetailResQuery>> GetActiveArticles(ContextType type, CancellationToken token)
        {
            return await BuildAggregateBasedOnContext(type).ToListAsync(token);
        }

        public async IAsyncEnumerable<ArticleDetailResQuery> GetActiveArticlesEnumerable(ContextType type, 
            [EnumeratorCancellation] CancellationToken token)
        {
            using var cursor = await BuildAggregateBasedOnContext(type).ToCursorAsync(token);
            
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
