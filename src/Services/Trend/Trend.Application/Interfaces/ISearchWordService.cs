using Dtos.Common;
using LanguageExt;
using LanguageExt.Common;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<PageResponseDto<SearchWordResDto>> FilterSearchWords(SearchWordFilterReqDto req, CancellationToken token);
        Task<List<SearchWordResDto>> GetActiveSearchWords(CancellationToken token);
        Task<List<SearchWordResDto>> GetDeactivatedSearchWords(CancellationToken token);
        Task<Either<TrendError, SearchWordResDto>> AddNewSearchWord(SearchWordAddReqDto instance, CancellationToken token);
        Task<Either<TrendError, SearchWordSyncDetailResDto>> GetSearchWordSyncStatistic(string wordId, CancellationToken token);
        Task<Either<TrendError, Unit>> AttachImageToSearchWord(SearchWordAttachImageReqDto instance, CancellationToken token);
        Task<Either<TrendError, Unit>> DeactivateSearchWord(string id, CancellationToken token);
        Task<Either<TrendError, Unit>> ActivateSearchWord(string id, CancellationToken token);
    }
}
