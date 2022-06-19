using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;

namespace Trend.Domain.Interfaces
{
    public interface ISyncStatusRepository : IRepository<SyncStatus>
    {
        Task<SyncStatus> GetLastValidSync();
        Task<List<SyncStatusWord>> GetSyncStatusWords(string syncStatusId);
    }
}
