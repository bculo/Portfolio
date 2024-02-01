using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Models
{
    public class TokenAuthorizationCodeResponse
    {
        [JsonProperty("access_token")] 
        public string AccessToken { get; set; } = default!;
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = default!;
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = default!;
        [JsonProperty("refresh_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }
    }
}
