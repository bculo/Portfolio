using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.Interfaces
{
    public interface ISyncService
    { 
        Task<List<SyncStatusResDto>> GetSyncStatuses(CancellationToken token);
        Task<long> GetSyncCount(CancellationToken token);
        Task<SyncStatusResDto> GetSync(string id, CancellationToken token);
        Task ExecuteSync(CancellationToken token);
        Task<List<SyncStatusWordResDto>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token);
        Task<PageResponseDto<SyncStatusResDto>> GetSyncStatusesPage(PageRequestDto request, CancellationToken token);
    }
}
