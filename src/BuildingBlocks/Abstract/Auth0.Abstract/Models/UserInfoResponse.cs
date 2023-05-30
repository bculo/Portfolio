using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0.Abstract.Models
{
    /// <summary>
    /// UserInfo endpoint response model
    /// </summary>
    public class UserInfoResponse
    {
        [JsonProperty("sub")]
        public string UserId { get; set; }
        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }
        [JsonProperty("name")]
        public string FullName { get; set; }
        [JsonProperty("preferred_username")]
        public string UserName { get; set; }
        [JsonProperty("given_name")]
        public string FirstName { get; set; }
        [JsonProperty("family_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
