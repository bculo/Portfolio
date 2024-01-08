using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Repositories;
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
