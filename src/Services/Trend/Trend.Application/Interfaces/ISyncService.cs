using LanguageExt;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface ISyncService
    { 
        Task<List<SyncStatusResDto>> GetAll(CancellationToken token = default);
        Task<long> GetAllCount(CancellationToken token = default);
        Task<Either<CoreError, SyncStatusResDto>> Get(GetSyncStatusReqDto req, CancellationToken token = default);
        Task<Either<CoreError, Unit>> ExecuteSync(CancellationToken token = default);
        Task<Either<CoreError, List<SyncSearchWordResDto>>> GetSyncSearchWords(
            SyncSearchWordsReqDto req, 
            CancellationToken token = default);
    }
}
