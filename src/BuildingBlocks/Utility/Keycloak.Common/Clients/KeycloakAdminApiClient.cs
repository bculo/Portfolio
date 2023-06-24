﻿using Keycloak.Common.Extensions;
using Keycloak.Common.Interfaces;
using Keycloak.Common.Models;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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

        public async Task<bool> CreateUser(string realm, string accessToken, UserRepresentation user)
        {
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);
            ArgumentNullException.ThrowIfNull(user);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var response = await http.PostAsJsonAsync($"{realm}/users", user);
            return response.IsSuccessStatusCode;
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

        public async Task<List<UserResponse>> GetUsers(string realm, string accessToken)
        {
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var response = await http.GetAsync($"{realm}/users");

            return await response.HandleResponse<List<UserResponse>>(_logger);
        }

        public async Task<bool> ImportRealm(string realmDataJson, string accessToken)
        {
            ArgumentNullException.ThrowIfNull(realmDataJson);
            ArgumentNullException.ThrowIfNull(accessToken);

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AdminApiEndpointBase);
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.Trim()}");

            var content = new StringContent(realmDataJson, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("", content);

            var result = await response.HandleResponse<string>(_logger);

            return (result != null) ? true : false;
        }
    }
}