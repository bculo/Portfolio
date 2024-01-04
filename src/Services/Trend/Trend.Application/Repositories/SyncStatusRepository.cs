using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;

namespace Trend.Application.Repositories
{
    public class SyncStatusRepository : MongoRepository<SyncStatus>, ISyncStatusRepository
    {
        public SyncStatusRepository(IMongoClient client, 
            IOptions<MongoOptions> options, 
            IDateTimeProvider timeProvider)
            : base(client, options, timeProvider)
        {
        }

        public Task<SyncStatus> GetLastValidSync(CancellationToken token)
        {
            var result = _collection.Find(t => t.TotalRequests > 0 && t.SucceddedRequests > 0)
                            .SortByDescending(i => i.Created)
                            .FirstOrDefault();

            return Task.FromResult(result);
        }

        public Task<List<SyncStatusWord>> GetSyncStatusWords(string syncStatusId, CancellationToken token)
        {
            var result = GetQueryable().Where(i => i.Id == syncStatusId)
                .SelectMany(i => i.UsedSyncWords)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
