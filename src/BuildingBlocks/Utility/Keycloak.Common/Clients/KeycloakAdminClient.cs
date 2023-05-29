using Keycloak.Common.Extensions;
using Keycloak.Common.Interfaces;
using Keycloak.Common.Models.Response.Users;
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
    internal class KeycloakAdminClient : IKeycloakAdminService
    {
        private readonly IHttpClientFactory _factory;
        private readonly KeycloakAdminApiOptions _options;
        private readonly ILogger<KeycloakAdminClient> _logger;

        public KeycloakAdminClient(IHttpClientFactory factory,
            IOptions<KeycloakAdminApiOptions> options,
            ILogger<KeycloakAdminClient> logger)
        {
            _factory = factory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<UserResponse> GetUserById(string realm, string accessToken, Guid userId)
        {
            _logger.LogTrace("Method {0} called", nameof(GetUserById));

            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            _logger.LogTrace("Sending request...");

            var response = await http.GetAsync($"{realm}/users/{userId}");

            return await response.HandleResponse<UserResponse>(_logger);
        }

        public async Task<List<UserResponse>> GetUsers(string realm, string accessToken)
        {
            _logger.LogTrace("Method {0} called", nameof(GetUsers));

            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            _logger.LogTrace("Sending request...");

            var response = await http.GetAsync($"{realm}/users");

            return await response.HandleResponse<List<UserResponse>>(_logger);
        }

        public async Task<bool> ImportRealm(string realmDataJson, string accessToken)
        {
            _logger.LogTrace("Method {0} called", nameof(GetUsers));

            ArgumentNullException.ThrowIfNull(realmDataJson);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            _logger.LogTrace("Sending request...");

            var content = new StringContent(realmDataJson, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("", content);

            var result = await response.HandleResponse<string>(_logger);

            return (result != null) ? true : false;
        }
    }
}
