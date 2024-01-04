using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Models.Services;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface ISearchEngine
    {
        string EngineName { get;  }
        Task<SearchEngineResult> Sync(Dictionary<ContextType, List<SearchEngineWord>> articleTypesToSync, CancellationToken token);
    }
}
