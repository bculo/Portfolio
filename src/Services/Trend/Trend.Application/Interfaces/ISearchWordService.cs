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
        Task<List<SearchWordDto>> GetSyncSettingsWords();
        Task<List<KeyValueElementDto>> GetAvailableSearchEngines();
        Task<SearchWordDto> AddNewSyncSetting(SearchWordCreateDto instance);
        Task RemoveSyncSetting(string id);
        Task<List<KeyValueElementDto>> GetAvaiableContextTypes();
    }
}
