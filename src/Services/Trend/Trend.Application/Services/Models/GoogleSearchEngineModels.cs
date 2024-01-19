using Trend.Application.Interfaces.Models;

namespace Trend.Application.Services.Models;

public class GoogleClientResponse
{
    public GoogleSearchEngineResponseDto? ClientResponse { get; init; }
    public string SearchWordId { get; init; }
    public bool Success => ClientResponse != null;
}

public class GoogleClientRequest
{
    public Task<GoogleSearchEngineResponseDto> RequestTask { get; set; }
    public string SearchWordId { get; init; }
}