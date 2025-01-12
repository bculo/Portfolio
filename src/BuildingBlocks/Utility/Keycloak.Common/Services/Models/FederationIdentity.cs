using System.Text.Json.Serialization;

namespace Keycloak.Common.Services.Models
{
    public class FederationIdentity
    {
        [JsonPropertyName("identityProvider")]
        public string IdentityProvider { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}
