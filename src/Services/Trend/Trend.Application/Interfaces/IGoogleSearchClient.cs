using Trend.Application.Interfaces.Models.Services.Google;

namespace Trend.Application.Interfaces
{
    public interface IGoogleSearchClient
    {
        Task<GoogleSearchEngineResponseDto> Search(string searchDefinition, CancellationToken token = default);
    }
}
