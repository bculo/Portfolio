using Newtonsoft.Json;

namespace Keycloak.Common.Services.Models
{
    public class UserResponse
    {
        [JsonProperty("id")]
        public Guid UserId { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("federatedIdentities")]
        public List<FederationIdentity> FederationIdentities { get; set; }
    }

}
