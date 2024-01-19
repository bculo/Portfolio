using Dtos.Common;
using LanguageExt;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<Either<CoreError, PageResponseDto<SearchWordResDto>>> Filter(
            FilterSearchWordsReqDto req, 
            CancellationToken token = default);
        Task<List<SearchWordResDto>> GetActiveItems(CancellationToken token = default);
        Task<List<SearchWordResDto>> GetDeactivatedItems(CancellationToken token = default);
        Task<Either<CoreError, SearchWordResDto>> CreateNew(
            AddWordReqDto instance, 
            CancellationToken token = default);
        Task<Either<CoreError, SearchWordSyncStatisticResDto>> GetSyncStatistic(
            SearchWordSyncStatisticReqDto req, 
            CancellationToken token = default);
        Task<Either<CoreError, Unit>> AttachImage(
            AttachImageToSearchWordReqDto instance, 
            CancellationToken token = default);
        Task<Either<CoreError, Unit>> Deactivate(DeactivateSearchWordReqDto req, CancellationToken token = default);
        Task<Either<CoreError, Unit>> Activate(ActivateSearchWordReqDto req, CancellationToken token = default);
    }
}


