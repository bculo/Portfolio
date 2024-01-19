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
    public class GoogleSearchClient : IGoogleSearchClient
    {
        private readonly GoogleSearchOptions _options;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GoogleSearchClient> _logger;

        public GoogleSearchClient(
            IHttpClientFactory clientFactory, 
            IOptions<GoogleSearchOptions> options, 
            ILogger<GoogleSearchClient> logger)
        {
            _options = options.Value;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<GoogleSearchEngineResponseDto> Search(string searchDefinition, CancellationToken token = default)
        {
            var client = _clientFactory.CreateClient(HttpClientNames.GOOGLE_CLIENT);

            var builder = new HttpQueryBuilder();
            builder.Add("key", _options.ApiKey);
            builder.Add("cx", _options.SearchEngineId);
            builder.Add("q", searchDefinition);
            
            var queryPart = builder.Build();
            var result = await client.GetAsync($"/customsearch/v1{queryPart}", token);
            return await result.HandleResponse<GoogleSearchEngineResponseDto>();
        }
    }
}
