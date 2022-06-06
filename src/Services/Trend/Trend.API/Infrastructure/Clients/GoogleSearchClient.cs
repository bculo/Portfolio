using HttpUtility.Abstract;
using Microsoft.Extensions.Options;
using Trend.API.Interfaces;
using Trend.API.Options;

namespace Trend.API.Infrastructure.Clients
{
    /// <summary>
    /// HTTP Client for Programmable Search Engine (JSON API) https://developers.google.com/custom-search/docs/overview
    /// To use GoogleSearchClient generate API key (JSON API section in above URL) and create Search engine ID (JSON API section - https://programmablesearchengine.google.com/cse/all)
    /// After configuration add GoogleOptions section in appsettings.json file (check required options in class GoogleSearchOptions)
    /// NOTE: Its free for 100 queries per day
    /// </summary>
    public class GoogleSearchClient : BaseHttpClient, IGoogleSearchService
    {
        private readonly HttpClient _client;
        private readonly GoogleSearchOptions _options;

        public GoogleSearchClient(HttpClient client, IOptions<GoogleSearchOptions> options) : base(client)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task<string> Search(string searchDefinition)
        {
            AddQueryParameter("key", _options.ApiKey); //API key
            AddQueryParameter("cx", _options.SearchEngineId); //Search engine ID
            AddQueryParameter("q", searchDefinition);

            var url = BuildUrlWithQueryParameters(_options.Uri);

            var result = await _client.GetAsync(url);

            var response = await result.Content.ReadAsStringAsync();

            return response;
        }
    }
}
