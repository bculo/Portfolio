using System.Text.Json.Serialization;

namespace Keycloak.Common.Services.Models;

public class UserProfileMetadata
{

    [JsonPropertyName("attributes")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public ICollection<UserProfileAttributeMetadata> Attributes { get; set; }

    private IDictionary<string, object> _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ??= new Dictionary<string, object>(); }
        set { _additionalProperties = value; }
    }

}