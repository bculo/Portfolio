using System.Text.Json.Serialization;

public partial class UserProfileAttributeMetadata
{

    [JsonPropertyName("name")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string Name { get; set; }

    [JsonPropertyName("displayName")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public string DisplayName { get; set; }

    [JsonPropertyName("required")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public bool Required { get; set; }

    [JsonPropertyName("readOnly")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public bool ReadOnly { get; set; }

    [JsonPropertyName("annotations")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public IDictionary<string, object> Annotations { get; set; }

    [JsonPropertyName("validators")]

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
    public IDictionary<string, object> Validators { get; set; }

    private IDictionary<string, object> _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }

}