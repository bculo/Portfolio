using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface ISearchEngine
    {
        string EngineName { get;  }
        Task<(SyncStatus syncIteration, List<Article> syncArticles)> Sync(Dictionary<ContextType, List<string>> articleTypesToSync, CancellationToken token);
    }
}
