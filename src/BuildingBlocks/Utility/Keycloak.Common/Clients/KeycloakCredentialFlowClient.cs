using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Constants;
using Keycloak.Common.Extensions;
using Keycloak.Common.Options;
using Keycloak.Common.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

            ArgumentNullException.ThrowIfNull(clientId);
            ArgumentNullException.ThrowIfNull(clientSecret);
            
            var http = _factory.CreateClient();
            
            var body = new List<KeyValuePair<string, string>>()
            {
                new ("client_id", clientId),
                new ("client_secret", clientSecret),
                new ("grant_type", KeycloakGrantTypeConstants.CLIENT_CREDENTIALS)
            };

            HttpContent content = new FormUrlEncodedContent(body);

            _logger.LogTrace("Sending request...");

            var uri = UriUtils.JoinUriSegments(_options.AuthorizationServerUrl, "protocol/openid-connect/token");
            var authorizationServerResponse = await http.PostAsync(uri, content);

            return await authorizationServerResponse.HandleResponse<TokenClientCredentialResponse>(_logger);
        }
    }
}
