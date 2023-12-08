using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Keycloak.Common.Models;

public class CreateUserErrorResponse
{
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
}