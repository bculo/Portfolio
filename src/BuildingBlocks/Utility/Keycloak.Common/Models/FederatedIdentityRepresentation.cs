using System.Text.Json.Serialization;

public class FederatedIdentityRepresentation
{

    [JsonPropertyName("identityProvider")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string IdentityProvider { get; set; }

    [JsonPropertyName("userId")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string UserId { get; set; }

    [JsonPropertyName("userName")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string UserName { get; set; }

    private IDictionary<string, object> _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ??= new Dictionary<string, object>(); }
        set { _additionalProperties = value; }
    }

}