using Keycloak.Common.Extensions;
using Keycloak.Common.Interfaces;
using Keycloak.Common.Models;
using Keycloak.Common.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Clients
{
    internal class KeycloakAdminApiClient : IKeycloakAdminService
    {
        private readonly IHttpClientFactory _factory;
        private readonly KeycloakAdminApiOptions _options;
        private readonly ILogger<KeycloakAdminApiClient> _logger;

        public KeycloakAdminApiClient(IHttpClientFactory factory,
            IOptions<KeycloakAdminApiOptions> options,
            ILogger<KeycloakAdminApiClient> logger)
        {
            _factory = factory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<(CreateUserErrorResponse Instance, bool Success)> CreateUser(string realm, string accessToken, UserRepresentation user)
        {
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);
            ArgumentNullException.ThrowIfNull(user);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var response = await http.PostAsJsonAsync($"{realm}/users", user);
            if (response.IsSuccessStatusCode)
            {
                return (null, true);
            }

            var bodyString = await response.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<CreateUserErrorResponse>(bodyString);
            return (body, false);
        }

        public async Task<UserResponse> GetUserById(string realm, string accessToken, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var response = await http.GetAsync($"{realm}/users/{userId}");

            return await response.HandleResponse<UserResponse>(_logger);
        }

        public async Task<List<UserResponse>> GetUsers(string realm, string accessToken, IReadOnlyDictionary<string, string> searchParams = null)
        {
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            string endpointUrl = $"{_options.AdminApiEndpointBase}{realm}/users";
            var stringUri = searchParams is null 
                ? endpointUrl
                : QueryHelpers.AddQueryString(endpointUrl, searchParams);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var response = await http.GetAsync(stringUri);
            return await response.HandleResponse<List<UserResponse>>(_logger);
        }
        
        public async Task<bool> UpdateUser(string realm, string accessToken, string userId, UserRepresentation user)
        {
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(user);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var response = await http.PutAsJsonAsync($"{realm}/users/{userId}", user);
            return response.IsSuccessStatusCode;
        }
    }
}
