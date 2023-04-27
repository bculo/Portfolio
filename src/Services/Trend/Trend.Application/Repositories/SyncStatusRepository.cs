using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trend.Application.Options;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class SyncStatusRepository : MongoRepository<SyncStatus>, ISyncStatusRepository
    {
        public SyncStatusRepository(IMongoClient client, IOptions<MongoOptions> options)
            : base(client, options)
        {
                
        }

        public Task<SyncStatus> GetLastValidSync()
        {
            var result = _collection.Find(t => t.TotalRequests > 0 && t.SucceddedRequests > 0)
                            .SortByDescending(i => i.Created)
                            .FirstOrDefault();

            return Task.FromResult(result);
        }

        public Task<List<SyncStatusWord>> GetSyncStatusWords(string syncStatusId)
        {
            var result = GetQueryable().Where(i => i.Id == syncStatusId)
                .SelectMany(i => i.UsedSyncWords)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
