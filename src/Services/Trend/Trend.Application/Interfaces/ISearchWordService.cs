using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<List<SearchWordResDto>> GetSyncSettingsWords(CancellationToken token);
        Task<List<KeyValueElementDto>> GetAvailableSearchEngines(CancellationToken token);
        Task<SearchWordResDto> AddNewSyncSetting(SearchWordCreateReqDto instance, CancellationToken token);
        Task RemoveSyncSetting(string id, CancellationToken token);
        Task<List<KeyValueElementDto>> GetAvailableContextTypes(CancellationToken token);
    }
}
