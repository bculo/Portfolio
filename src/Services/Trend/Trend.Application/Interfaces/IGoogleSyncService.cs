using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Models.Service.Google;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IGoogleSyncService
    {
        Task<GoogleSyncResult> Sync(Dictionary<ContextType, List<string>> articleTypesToSync);
    }
}
