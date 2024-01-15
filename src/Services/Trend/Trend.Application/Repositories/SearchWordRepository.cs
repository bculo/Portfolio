using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
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
    public class SearchWordRepository : MongoAuditableRepository<SearchWord>, ISearchWordRepository
    {
        public SearchWordRepository(IMongoClient client, 
            IOptions<MongoOptions> options,
            IDateTimeProvider timeProvider) 
            : base(client, options, timeProvider)
        {
        }

        public async Task<SearchWordSyncDetailResQuery> GetSearchWordSyncInfo(string searchWordId, CancellationToken token)
        {
            var syncStatusCollection = GetCollection<SyncStatus>();
            
            var itemTask = syncStatusCollection.Aggregate()
                .Unwind<SyncStatus, SyncStatusUnwind>(x => x.UsedSyncWords)
                .Match(x => x.UsedSyncWords.WordId == searchWordId)
                .Lookup<SyncStatusUnwind, SearchWord, SearchWordSyncStatusLookup>(_collection,
                    x => x.UsedSyncWords.WordId,
                    y => y.Id,
                    z => z.SearchWords)
                .Unwind<SearchWordSyncStatusLookup, SearchWordSyncStatusUnwind>(x => x.SearchWords)
                .Group(x => x.SearchWords.Id, item => new SearchWordSyncDetailResQuery
                {
                    WordId = item.Key,
                    Count = item.Count()
                })
                .FirstOrDefaultAsync(token);

            var totalCountTask = syncStatusCollection.CountDocumentsAsync(x => true, new CountOptions(), token);

            await Task.WhenAll(itemTask, totalCountTask);

            var response = itemTask.Result;
            response.TotalCount = totalCountTask.Result;
            return response;
        }

        public Task<bool> IsDuplicate(string searchWord, SearchEngine engine, CancellationToken token)
        {
            var instance = _collection
                .Find(i => string.Equals(i.Word, searchWord, StringComparison.CurrentCultureIgnoreCase) && i.Engine == engine)
                .FirstOrDefault();
            
            return Task.FromResult(instance != null);
        }

        public async Task<PageResQuery<SearchWord>> Filter(SearchWordFilterReqQuery req, CancellationToken token)
        {
            var searchBuilder = Builders<SearchWord>.Filter;
            var searchFilter = FilterDefinition<SearchWord>.Empty;
            if (req.SearchEngine.IsRelevantForFilter())
            {
                searchFilter &= searchBuilder.Eq(i => i.Engine.Id, req.SearchEngine.Id);
            }
            
            if (req.Active.IsRelevantForFilter())
            {
                searchFilter &= searchBuilder.Eq(i => i.IsActive, req.Active.Value);
            }
            
            if (req.ContextType.IsRelevantForFilter())
            {
                searchFilter &= searchBuilder.Eq(i => i.Type.Id, req.ContextType.Id);
            }

            if (!string.IsNullOrWhiteSpace(req.Query))
            {
                searchFilter &= searchBuilder.Regex(i => i.Word, new BsonRegularExpression(req.Query));
            }
            
            var sortBuilder = Builders<SearchWord>.Sort;
            var sortFilter = req.Sort == SortType.Asc
                ? sortBuilder.Ascending(x => x.Created)
                : sortBuilder.Descending(x => x.Created);
            
            var countTask = _collection.Find(searchFilter).CountDocumentsAsync(token);
            var collectionTask =  _collection.Find(searchFilter)
                .Sort(sortFilter)
                .Skip(req.Skip)
                .Limit(req.Take)
                .ToListAsync(token);

            await Task.WhenAll(countTask, collectionTask);
            
            return new PageResQuery<SearchWord>(countTask.Result, collectionTask.Result);
        }
    }
}
