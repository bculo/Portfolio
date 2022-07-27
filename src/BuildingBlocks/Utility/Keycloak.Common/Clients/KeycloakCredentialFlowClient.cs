using Ardalis.GuardClauses;
using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Constants;
using Keycloak.Common.Extensions;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Keycloak.Common.Clients
{
    internal class KeycloakCredentialFlowClient : IAuth0ClientCredentialFlowService
    {
        private readonly IHttpClientFactory _factory;
        private readonly KeycloakClientCredentialFlowOptions _options;
        private readonly ILogger<KeycloakCredentialFlowClient> _logger;

        public KeycloakCredentialFlowClient(IHttpClientFactory factory, 
            IOptions<KeycloakClientCredentialFlowOptions> options,
            ILogger<KeycloakCredentialFlowClient> logger)
        {
            _factory = factory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<TokenClientCredentialResponse> GetToken(string clientId, string clientSecret, IEnumerable<string>? scopes = null)
        {
            _logger.LogTrace("Method {0} called", nameof(GetToken));

            Guard.Against.NullOrEmpty(clientId);
            Guard.Against.NullOrEmpty(clientSecret);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);

            var body = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", KeycloakGrantTypeConstants.CLIENT_CREDENTIALS)
            };

            HttpContent content = new FormUrlEncodedContent(body);

            _logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.PostAsync("protocol/openid-connect/token", content);

            return await authorizationServerResponse.HandleResponse<TokenClientCredentialResponse>(_logger);
        }
    }
}
