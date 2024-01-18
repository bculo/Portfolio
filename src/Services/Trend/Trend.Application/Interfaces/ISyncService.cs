using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.Interfaces
{
    public interface ISyncService
    { 
        Task<List<SyncStatusResDto>> GetSyncStatuses(CancellationToken token = default);
        Task<long> GetSyncCount(CancellationToken token = default);
        Task<SyncStatusResDto> GetSync(string id, CancellationToken token = default);
        Task ExecuteSync(CancellationToken token = default);
        Task<List<SyncStatusWordResDto>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token = default);
    }
}
