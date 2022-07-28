using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests.Models
{
    public class UserRealmAccess
    {
        public List<string> roles { get; set; }
    }

    public class UserAccessToken
    {
        public int exp { get; set; }
        public int iat { get; set; }
        public string jti { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string typ { get; set; }
        public string azp { get; set; }
        public string session_state { get; set; }
        public string acr { get; set; }

        [JsonProperty("allowed-origins")]
        public List<string> AllowedOrigins { get; set; }
        public UserRealmAccess realm_access { get; set; }
        public Dictionary<string, string[]> resource_access { get; set; }
        public string scope { get; set; }
        public string sid { get; set; }
        public bool email_verified { get; set; }
        public string name { get; set; }
        public string preferred_username { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
    }

    public class ClientRealmAccess
    {
        public List<string> roles { get; set; }
    }

    public class ClientAccessToken
    {
        public int exp { get; set; }
        public int iat { get; set; }
        public string jti { get; set; }
        public string iss { get; set; }
        public string sub { get; set; }
        public string typ { get; set; }
        public string azp { get; set; }
        public string acr { get; set; }
        public ClientRealmAccess realm_access { get; set; }
        public string scope { get; set; }
        public string clientHost { get; set; }
        public string clientId { get; set; }
        public bool email_verified { get; set; }
        public string preferred_username { get; set; }
        public string clientAddress { get; set; }
    }
}
