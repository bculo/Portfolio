using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Models.Requests
{
    internal class KeycloakClientCredentialsFlowRequest
    {
        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }

        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }
    }
}
