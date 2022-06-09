using Http.Common.Abstract;
using Http.Common.Extensions;
using HttpUtility.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces;
using Trend.Application.Models.Dtos.Google;
using Trend.Application.Options;

namespace Trend.Application.Clients
{
    /// <summary>
    /// HTTP Client for Programmable Search Engine (JSON API) https://developers.google.com/custom-search/docs/overview
    /// To use GoogleSearchClient generate API key (JSON API section in above URL) and create Search engine ID (JSON API section - https://programmablesearchengine.google.com/cse/all)
    /// After configuration add GoogleOptions section in appsettings.json file (check required options in class GoogleSearchOptions)
    /// NOTE: Its free for 100 queries per day
    /// API KEY located -> https://console.cloud.google.com/apis/dashboard?project=trendsearcher-1654511636808
    /// </summary>
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

            _logger.LogTrace("Sending request");

            var result = await client.GetAsync(url);

            _logger.LogTrace("Response Received");

            return await result.HandleResponse<GoogleSearchEngineResponseDto>();
        }
    }
}
