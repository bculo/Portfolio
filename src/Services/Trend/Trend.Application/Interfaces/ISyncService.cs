using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Models.Service.Google;
using Trend.Domain.Entities;

namespace Trend.Application.Interfaces
{
    public interface ISyncService
    {
        Task<List<SyncSettingDto>> GetSyncSettingsWords();
        Task<List<SyncStatusDto>> GetSyncStatuses();
        Task<GoogleSyncResult> ExecuteGoogleSync();
        Task<List<KeyValueElementDto>> GetAvailableSearchEngines();
        Task<SyncSettingDto> AddNewSyncSetting(SyncSettingCreateDto instance); 
        Task RemoveSyncSetting(string id);
        Task<List<KeyValueElementDto>> GetAvaiableContextTypes();
    }
}
