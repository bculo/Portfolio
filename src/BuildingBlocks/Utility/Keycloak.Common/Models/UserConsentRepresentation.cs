using System.Text.Json.Serialization;

public class UserConsentRepresentation
{

    [JsonPropertyName("clientId")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string ClientId { get; set; }

    [JsonPropertyName("grantedClientScopes")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public ICollection<string> GrantedClientScopes { get; set; }

    [JsonPropertyName("createdDate")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public long CreatedDate { get; set; }

    [JsonPropertyName("lastUpdatedDate")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public long LastUpdatedDate { get; set; }

    [JsonPropertyName("grantedRealmRoles")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public ICollection<string> GrantedRealmRoles { get; set; }

    private IDictionary<string, object> _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }

}