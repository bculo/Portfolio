using Http.Common.Builders;
using Http.Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Constants;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;

namespace Trend.Application.Services
{
    public class GoogleSearchClient(
        IHttpClientFactory clientFactory,
        IOptions<GoogleSearchOptions> options,
        ILogger<GoogleSearchClient> logger)
        : IGoogleSearchClient
    {
        private readonly GoogleSearchOptions _options = options.Value;
        private readonly ILogger<GoogleSearchClient> _logger = logger;

        public async Task<GoogleSearchEngineResponseDto> Search(string searchDefinition, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient(HttpClientNames.GoogleClient);

            var builder = new HttpQueryBuilder();
            builder.Add("key", _options.ApiKey);
            builder.Add("cx", _options.SearchEngineId);
            builder.Add("q", searchDefinition);
            
            var queryPart = builder.Build();
            var result = await client.GetAsync($"/customsearch/v1{queryPart}", token);
            return await result.ExtractContentFromResponse<GoogleSearchEngineResponseDto>();
        }
    }
}
