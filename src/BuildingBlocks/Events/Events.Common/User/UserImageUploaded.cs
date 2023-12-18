using Newtonsoft.Json;

namespace Events.Common.User;

public class UserImageUploaded
{
    public UserImageUploadedBody RawMessage { get; set; }
    
    public UserImageUploadedBody Body { get; set; }
    
    public Dictionary<string, string> Headers { get; set; } = new();
    
    public Dictionary<string, string> Path { get; set; } = new();
    
    public string ContentType { get; set; }
    
    public Guid MessageId { get; set; }
    
    public Guid CorrelationId { get; set; }
    
    public bool Processed { get; set; }
    
    public bool Commited { get; set; }
}

public class UserImageUploadedBody
{
    public Guid UserId { get; set; }
    public string ImageName { get; set; }
    public string Uri { get; set; }
    public string UserName { get; set; }
}