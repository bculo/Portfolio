using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface ISearchEngine
    {
        Task<bool> Sync(Dictionary<ContextType, List<string>> articleTypesToSync, CancellationToken token);
    }
}
