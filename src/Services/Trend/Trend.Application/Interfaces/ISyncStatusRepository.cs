using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;

namespace Trend.Application.Interfaces
{
    public interface ISyncStatusRepository : IRepository<SyncStatus>
    {
        Task<SyncStatus> GetLastValidSync(CancellationToken token);
        Task<List<SyncStatusWord>> GetSyncStatusWords(string syncStatusId, CancellationToken token);
    }
}
