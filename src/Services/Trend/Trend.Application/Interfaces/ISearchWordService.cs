using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.SearchWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces
{
    public interface ISearchWordService
    {
        Task<List<SearchWordDto>> GetSyncSettingsWords(CancellationToken token);
        Task<List<KeyValueElementDto>> GetAvailableSearchEngines(CancellationToken token);
        Task<SearchWordDto> AddNewSyncSetting(SearchWordCreateDto instance, CancellationToken token);
        Task RemoveSyncSetting(string id, CancellationToken token);
        Task<List<KeyValueElementDto>> GetAvailableContextTypes(CancellationToken token);
    }
}
