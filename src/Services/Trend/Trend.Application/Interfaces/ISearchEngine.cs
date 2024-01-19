using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface ISearchEngine
    {
        string EngineName { get;  }
        Task<SearchEngineRes> Sync(
            Dictionary<ContextType, List<SearchEngineReq>> searchWordsByCategory, 
            CancellationToken token = default);
    }
}
