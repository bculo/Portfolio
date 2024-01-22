using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Constants;
using Keycloak.Common.Extensions;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Clients
{
    internal class KeycloakResourceOwnerPasswordFlowClient : IAuth0PasswordCredentialFlowService
    {
        private readonly KeycloakTokenOptions _options;
        private readonly ILogger<KeycloakResourceOwnerPasswordFlowClient> _logger;
        private readonly IHttpClientFactory _factory;

        public KeycloakResourceOwnerPasswordFlowClient(IHttpClientFactory factory,
            IOptions<KeycloakTokenOptions> options,
            ILogger<KeycloakResourceOwnerPasswordFlowClient> logger)
        {
            _options = options.Value;
            _logger = logger;
            _factory = factory;
        }

        public async Task<TokenAuthorizationCodeResponse> GetToken(string clientId, string username, string password, IEnumerable<string> scopes = null)
        {
            _logger.LogTrace("Method {0} called", nameof(GetToken));

            ArgumentNullException.ThrowIfNull(clientId);
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);

            var body = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", KeycloakGrantTypeConstants.OWNER_CREDENTIALS),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            };

            HttpContent content = new FormUrlEncodedContent(body);

            _logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.PostAsync("protocol/openid-connect/token", content);

            return await authorizationServerResponse.HandleResponse<TokenAuthorizationCodeResponse>(_logger);
        }
    }
}
