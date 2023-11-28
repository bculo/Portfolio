using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Models.Service.Intern.Google;
using Trend.Domain.Entities;

namespace Trend.Application.Interfaces
{
    public interface ISyncService
    { 
        Task<List<SyncStatusDto>> GetSyncStatuses(CancellationToken token);
        Task<SyncStatusDto> GetSync(string id, CancellationToken token);
        Task ExecuteSync(CancellationToken token);
        Task<List<SyncStatusWordDto>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token);
        Task<PageResponseDto<SyncStatusDto>> GetSyncStatusesPage(PageRequestDto request, CancellationToken token);
    }
}
