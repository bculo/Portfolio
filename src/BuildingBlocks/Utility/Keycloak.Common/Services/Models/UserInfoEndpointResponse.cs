using System.Text.Json.Serialization;

namespace Keycloak.Common.Services.Models;

internal class UserInfoEndpointResponse
{
    [JsonPropertyName("client_roles")]
    public List<string> ClientRoles { get; set; } // Corresponds to "client_roles"
    
    [JsonPropertyName("roles")]
    public List<string> RealmRoles { get; set; } // Corresponds to "roles"
    
    [JsonPropertyName("email")]
    public string Email { get; set; } // Corresponds to "email"
}