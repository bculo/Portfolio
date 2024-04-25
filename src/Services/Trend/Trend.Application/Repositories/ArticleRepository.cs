using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
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
            IClientSessionHandle clientSession,
            IMongoDatabase database) 
            : base(client, options, provider, clientSession, database) {}

        private IAggregateFluent<ArticleDetailResQuery> BuildAggregateBasedOnContext()
        {
            var wordCollection = GetCollection<SearchWord>();
            
            var aggregate = Collection.Aggregate(ClientSession)
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
                    ArticleUrl = x.ArticleUrl,
                    IsActive = x.IsActive
                });
            
            return aggregate;
        }
        
        public async Task<List<ArticleDetailResQuery>> GetActive(ContextType type, 
            CancellationToken token = default)
        {
            return await BuildAggregateBasedOnContext()
                .Match(x => x.IsActive == true && x.ContextType == type.Value)
                .ToListAsync(token);
        }

        public async IAsyncEnumerable<ArticleDetailResQuery> GetActiveEnumerable(ContextType type, 
            [EnumeratorCancellation] CancellationToken token = default)
        {
            using var cursor = await BuildAggregateBasedOnContext()
                .Match(x => x.IsActive == true && x.ContextType == type.Value)
                .ToCursorAsync(token);
            
            while (await cursor.MoveNextAsync(token))
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public async Task<PageResQuery<ArticleDetailResQuery>> Filter(FilterArticlesReqQuery search, CancellationToken token)
        {
            var searchBuilder = Builders<ArticleDetailResQuery>.Filter;
            var searchFilter = FilterDefinition<ArticleDetailResQuery>.Empty;
            if (search.Activity != ActiveFilter.All)
            {
                var value = search.Activity == ActiveFilter.Active;
                searchFilter &= searchBuilder.Eq(i => i.IsActive, value);
            }
            
            if (search.Context != ContextType.All)
            {
                searchFilter &= searchBuilder.Eq(i => i.ContextType.Value, search.Context.Value);
            }

            if (!string.IsNullOrWhiteSpace(search.Query))
            {
                searchFilter &= searchBuilder.Regex(i => i.Title, new BsonRegularExpression(search.Query));
            }
            
            var query = BuildAggregateBasedOnContext().Match(searchFilter);

            var count = query.Count().FirstOrDefault()?.Count ?? 0;
            var collection = await query.Skip(search.Skip)
                .Limit(search.Take)
                .ToListAsync(token);
            
            return new PageResQuery<ArticleDetailResQuery>(count, collection);
        }
    }
}
