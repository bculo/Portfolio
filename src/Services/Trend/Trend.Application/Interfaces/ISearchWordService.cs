using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<PageResponseDto<SearchWordResDto>> FilterSearchWords(SearchWordFilterReqDto req, CancellationToken token);
        Task<List<SearchWordResDto>> GetActiveSearchWords(CancellationToken token);
        Task<List<SearchWordResDto>> GetDeactivatedSearchWords(CancellationToken token);
        Task<SearchWordResDto> AddNewSearchWord(SearchWordAddReqDto instance, CancellationToken token);
        Task AttachImageToSearchWord(SearchWordAttachImageReqDto instance, CancellationToken token);
        Task DeactivateSearchWord(string id, CancellationToken token);
        Task ActivateSearchWord(string id, CancellationToken token);
    }
}
