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

namespace Keycloak.Common.Services
{
    internal class KeycloakResourceOwnerPasswordFlowClient : IAuth0ResourceOwnerPasswordFlowService
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

        public async Task<TokenAuthorizationCodeResponse> GetToken(string clientId, 
            string username, 
            string password, 
            IEnumerable<string>? scopes = null)
        {
            _logger.LogTrace("Method {Method} called", nameof(GetToken));

            ArgumentNullException.ThrowIfNull(clientId);
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);

            var body = new List<KeyValuePair<string, string>>()
            {
                new ("grant_type", KeycloakGrantTypeConstants.OWNER_CREDENTIALS),
                new ("client_id", clientId),
                new ("username", username),
                new ("password", password)
            };

            HttpContent content = new FormUrlEncodedContent(body);

            _logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.PostAsync("protocol/openid-connect/token", content);

            return await authorizationServerResponse.HandleResponse<TokenAuthorizationCodeResponse>(_logger);
        }
    }
}
