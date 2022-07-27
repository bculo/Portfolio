using Ardalis.GuardClauses;
using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
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
    internal class KeycloakUserInfoClient : IOpenIdUserInfoService
    {
        private readonly KeycloakUserInfoOptions _options;
        private readonly ILogger<KeycloakOwnerCredentialFlowClient> _logger;
        private readonly IHttpClientFactory _factory;

        public KeycloakUserInfoClient(IOptions<KeycloakUserInfoOptions> options,
            ILogger<KeycloakOwnerCredentialFlowClient> logger,
            IHttpClientFactory factory)
        {
            _options = options.Value;
            _logger = logger;
            _factory = factory;
        }

        public async Task<UserInfoResponse> GetUserInfo(string accessToken)
        {
            _logger.LogTrace("Method {0} in {1} called", nameof(GetUserInfo), nameof(KeycloakUserInfoClient));

            Guard.Against.NullOrEmpty(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            _logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.GetAsync("protocol/openid-connect/userinfo");

            _logger.LogTrace("Response received...");

            if (!authorizationServerResponse.IsSuccessStatusCode)
            {
                var errorResponse = await authorizationServerResponse.Content.ReadAsStringAsync();

                _logger.LogWarning("User info request failed with status code {0}. Reason: {1}, Details {2}",
                    authorizationServerResponse.StatusCode,
                    authorizationServerResponse.ReasonPhrase,
                    errorResponse);

                return null;
            }

            _logger.LogTrace("Reading response...");

            var responseJson = await authorizationServerResponse.Content.ReadAsStringAsync();

            _logger.LogTrace("Parsing json response...");

            return JsonConvert.DeserializeObject<UserInfoResponse>(responseJson);
        }
    }
}
