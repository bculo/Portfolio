using Http.Common.Abstract;
using Http.Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Interfaces;
using Trend.Application.Models.Dtos.Google;
using Trend.Application.Options;

namespace Trend.Application.Clients
{
    public class GoogleSearchClient : BaseHttpFactoryClient, IGoogleSearchClient
    {
        private readonly GoogleSearchOptions _options;
        private readonly ILogger<GoogleSearchClient> _logger;

        public GoogleSearchClient(
            IHttpClientFactory clientFactory, 
            IOptions<GoogleSearchOptions> options, 
            ILogger<GoogleSearchClient> logger) 
            : base(clientFactory)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<GoogleSearchEngineResponseDto> Search(string searchDefinition)
        {
            _logger.LogTrace("Search method in GoogleSearchClient called");

            AddQueryParameter("key", _options.ApiKey);
            AddQueryParameter("cx", _options.SearchEngineId);
            AddQueryParameter("q", searchDefinition);

            var url = BuildUrlWithQueryParameters(_options.Uri);
            var client = CreateNewClient();
            var result = await client.GetAsync(url);
            return await result.HandleResponse<GoogleSearchEngineResponseDto>();
        }
    }
}
