using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<List<SearchWordResDto>> GetSearchWords(CancellationToken token);
        Task<SearchWordResDto> AddNewSearchWord(SearchWordAddReqDto instance, CancellationToken token);
        Task AttachImageToSearchWord(SearchWordAttachImageReqDto instance, CancellationToken token);
        Task DeactivateSearchWord(string id, CancellationToken token);
        Task ActivateSearchWord(string id, CancellationToken token);
        Task<List<KeyValueElementDto>> GetAvailableContextTypes(CancellationToken token);
        Task<List<KeyValueElementDto>> GetAvailableSearchEngines(CancellationToken token);
    }
}
