using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Constants;
using Keycloak.Common.Extensions;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Keycloak.Common.Services
{
    internal class KeycloakResourceOwnerPasswordFlowClient(
        IHttpClientFactory factory,
        IOptions<KeycloakTokenOptions> options,
        ILogger<KeycloakResourceOwnerPasswordFlowClient> logger)
        : IAuth0ResourceOwnerPasswordFlowService
    {
        private readonly KeycloakTokenOptions _options = options.Value;

        public async Task<TokenAuthorizationCodeResponse> GetToken(string clientId, 
            string username, 
            string password, 
            IEnumerable<string>? scopes = null)
        {
            logger.LogTrace("Method {Method} called", nameof(GetToken));

            ArgumentNullException.ThrowIfNull(clientId);
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            var http = factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);

            var body = new List<KeyValuePair<string, string>>()
            {
                new ("grant_type", KeycloakGrantTypeConstants.OwnerCredentials),
                new ("client_id", clientId),
                new ("username", username),
                new ("password", password)
            };

            HttpContent content = new FormUrlEncodedContent(body);

            logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.PostAsync("protocol/openid-connect/token", content);

            return await authorizationServerResponse.HandleResponse<TokenAuthorizationCodeResponse>(logger);
        }
    }
}
