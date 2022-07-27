using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Models
{
    public class TokenClientCredentialResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
