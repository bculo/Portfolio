using Trend.Domain.Entities;

namespace Trend.Application.Interfaces
{
    public interface ISyncStatusRepository : IRepository<SyncStatus>
    {
        Task<SyncStatus?> GetLastValidSync(CancellationToken token = default);
        Task<List<SyncStatusWord>> GetSyncStatusWords(string syncStatusId, CancellationToken token = default);
    }
}
