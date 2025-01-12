using System.Text.Json.Serialization;

namespace Keycloak.Common.Services.Models;

public class SocialLinkRepresentation
{

    [JsonPropertyName("socialProvider")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string SocialProvider { get; set; }

    [JsonPropertyName("socialUserId")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string SocialUserId { get; set; }

    [JsonPropertyName("socialUsername")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string SocialUsername { get; set; }

    private IDictionary<string, object> _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ??= new Dictionary<string, object>(); }
        set { _additionalProperties = value; }
    }

}