using Dtos.Common;
using LanguageExt;
using LanguageExt.Common;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<PageResponseDto<SearchWordResDto>> FilterSearchWords(SearchWordFilterReqDto req, CancellationToken token = default);
        Task<List<SearchWordResDto>> GetActiveSearchWords(CancellationToken token = default);
        Task<List<SearchWordResDto>> GetDeactivatedSearchWords(CancellationToken token = default);
        Task<Either<CoreError, SearchWordResDto>> AddNewSearchWord(SearchWordAddReqDto instance, CancellationToken token = default);
        Task<Either<CoreError, SearchWordSyncDetailResDto>> GetSearchWordSyncStatistic(string wordId, CancellationToken token = default);
        Task<Either<CoreError, Unit>> AttachImageToSearchWord(SearchWordAttachImageReqDto instance, CancellationToken token = default);
        Task<Either<CoreError, Unit>> DeactivateSearchWord(string id, CancellationToken token = default);
        Task<Either<CoreError, Unit>> ActivateSearchWord(string id, CancellationToken token = default);
    }
}
