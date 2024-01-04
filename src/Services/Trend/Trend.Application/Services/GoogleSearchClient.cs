using Http.Common.Abstract;
using Http.Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Services.Google;

namespace Trend.Application.Services
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
            AddQueryParameter("key", _options.ApiKey);
            AddQueryParameter("cx", _options.SearchEngineId);
            AddQueryParameter("q", searchDefinition);

            var url = BuildUrlWithQueryParameters($"{_options.Uri}/customsearch/v1");
            var client = CreateNewClient();
            var result = await client.GetAsync(url);
            return await result.HandleResponse<GoogleSearchEngineResponseDto>();
        }
    }
}
