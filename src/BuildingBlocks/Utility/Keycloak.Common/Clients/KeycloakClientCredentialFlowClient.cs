using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Constants;
using Keycloak.Common.Models.Requests;
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
    internal class KeycloakClientCredentialFlowClient : IAuth0ClientCredentialFlowService
    {
        private readonly IHttpClientFactory _factory;
        private readonly KeycloackClientCredentialFlowOptions _options;
        private readonly ILogger<KeycloakClientCredentialFlowClient> _logger;

        public KeycloakClientCredentialFlowClient(IHttpClientFactory factory, 
            IOptions<KeycloackClientCredentialFlowOptions> options,
            ILogger<KeycloakClientCredentialFlowClient> logger)
        {
            _factory = factory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<ClientCredentialsFlowResponse> GetTokenUsingClientCredentialsFlow(IEnumerable<string>? scopes = null)
        {
            _logger.LogTrace("Method {0} called", nameof(GetTokenUsingClientCredentialsFlow));

            var http = _factory.CreateClient();

            http.BaseAddress = new Uri(_options.AuthorizationServerUrl);

            JsonContent content = JsonContent.Create(new KeycloakClientCredentialsFlowRequest
            {
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                GrantType = KeycloakGrantTypeConstants.CLIENT_CREDENTIALS,
                Scope = DefineScope(scopes)
            }, mediaType: new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded")); ;

            _logger.LogTrace("Sending request...");

            var authorizationServerResponse = await http.PostAsync("/protocol/openid-connect/token", content);

            _logger.LogTrace("Response received...");

            if (authorizationServerResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning("Client credentials flow request failed with status code {0}. Reason: {1}", 
                    authorizationServerResponse.StatusCode, 
                    authorizationServerResponse.ReasonPhrase);
                return null;
            }

            _logger.LogTrace("Reading response...");

            var responseJson = await authorizationServerResponse.Content.ReadAsStringAsync();

            _logger.LogTrace("Parsing json response...");

            return JsonConvert.DeserializeObject<ClientCredentialsFlowResponse>(responseJson);
        }

        private string DefineScope(IEnumerable<string>? scopes)
        {
            if(scopes == null || !scopes.Any())
            {
                return null;
            }

            return String.Join(" ", scopes.Select(i => i?.Trim()));
        }
    }
}
