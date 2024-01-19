using Trend.Application.Interfaces.Models;

namespace Trend.Application.Interfaces
{
    public interface IGoogleSearchClient
    {
        Task<GoogleSearchEngineResponseDto> Search(string searchDefinition, CancellationToken token = default);
    }
}
