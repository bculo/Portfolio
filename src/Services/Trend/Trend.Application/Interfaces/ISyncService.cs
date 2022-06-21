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
        Task<List<SyncStatusDto>> GetSyncStatuses();
        Task<SyncStatusDto> GetSync(string id);
        Task<GoogleSyncResult> ExecuteGoogleSync();
        Task<List<SyncStatusWordDto>> GetSyncStatusSearchWords(string syncStatusId);
        Task<PageResponseDto<SyncStatusDto>> GetSyncStatusesPage(PageRequestDto request);
    }
}
