using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
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
    internal class KeycloakUserInfoClient : IOpenIdUserInfoService
    {
        private readonly KeycloakUserInfoOptions _options;
        private readonly ILogger<KeycloakUserInfoClient> _logger;
        private readonly IHttpClientFactory _factory;

        public KeycloakUserInfoClient(IOptions<KeycloakUserInfoOptions> options,
            ILogger<KeycloakUserInfoClient> logger,
            IHttpClientFactory factory)
        {
            _options = options.Value;
            _logger = logger;
            _factory = factory;
        }

        public async Task<UserInfoResponse> GetUserInfo(string accessToken)
        {
            _logger.LogTrace("Method {0} in {1} called", nameof(GetUserInfo), nameof(KeycloakUserInfoClient));

            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            _logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.GetAsync("protocol/openid-connect/userinfo");

            return await authorizationServerResponse.HandleResponse<UserInfoResponse>(_logger);
        }
    }
}
