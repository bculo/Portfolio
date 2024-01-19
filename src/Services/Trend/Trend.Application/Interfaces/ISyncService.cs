using Dtos.Common;
using LanguageExt;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface ISyncService
    { 
        Task<List<SyncStatusResDto>> GetSyncStatuses(CancellationToken token = default);
        Task<long> GetSyncCount(CancellationToken token = default);
        Task<Either<CoreError, SyncStatusResDto>> GetSync(string id, CancellationToken token = default);
        Task<Either<CoreError, Unit>> ExecuteSync(CancellationToken token = default);
        Task<Either<CoreError, List<SyncStatusWordResDto>>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token = default);
    }
}
