using Microsoft.EntityFrameworkCore;
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
            IDateTimeProvider timeProvider,
            IClientSessionHandle clientSession)
            : base(client, options, timeProvider, clientSession)
        {
        }

        public async Task<SyncStatus?> GetLastValidSync(CancellationToken token = default)
        {
            return await Collection.Find(ClientSession, t => t.TotalRequests > 0 && t.SucceddedRequests > 0)
                            .SortByDescending(i => i.Created)
                            .FirstOrDefaultAsync(token);
        }

        public async Task<List<SyncStatusWord>> GetSyncStatusWords(string syncStatusId, CancellationToken token = default)
        {
            var result = await GetQueryable().Where(i => i.Id == syncStatusId)
                .SelectMany(i => i.UsedSyncWords)
                .ToListAsync(token);

            return result;
        }
    }
}
