using Newtonsoft.Json;

namespace Events.Common.User;

public class UserImageUploaded
{
    [JsonProperty("raw_message")]
    public UserImageUploadedBody RawMessage { get; set; }
    
    [JsonProperty("body")]
    public UserImageUploadedBody Body { get; set; }

    [JsonProperty("headers")] 
    public Dictionary<string, string> Headers { get; set; } = new();
    
    [JsonProperty("path")]
    public Dictionary<string, string> Path { get; set; } = new();
    
    [JsonProperty("content_type")]
    public string ContentType { get; set; }
    
    [JsonProperty("message_id")]
    public Guid MessageId { get; set; }
    
    [JsonProperty("correlation_id")]
    public Guid CorrelationId { get; set; }
    
    [JsonProperty("processed")]
    public bool Processed { get; set; }
    
    [JsonProperty("commited")]
    public bool Commited { get; set; }
}

public class UserImageUploadedBody
{
    [JsonProperty("user_id")]
    public Guid UserId { get; set; }
    public string ImageName { get; set; }
    public string Uri { get; set; }
}