using Newtonsoft.Json;

namespace Keycloak.Common.Services.Models
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
