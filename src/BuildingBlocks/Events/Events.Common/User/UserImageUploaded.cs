using Newtonsoft.Json;

namespace Events.Common.User;

public class UserImageUploaded
{
    public UserImageUploadedBody RawMessage { get; set; } = default!;
    
    public UserImageUploadedBody Body { get; set; } = default!;
    
    public Dictionary<string, string> Headers { get; set; } = new();
    
    public Dictionary<string, string> Path { get; set; } = new();
    
    public string ContentType { get; set; } = default!;
    
    public Guid MessageId { get; set; }
    
    public Guid CorrelationId { get; set; } = default!;
    
    public bool Processed { get; set; }
    
    public bool Commited { get; set; } = default!;
}

public class UserImageUploadedBody
{
    public Guid UserId { get; set; }
    public string ImageName { get; set; } = default!;
    public string Uri { get; set; } = default!;
    public string UserName { get; set; } = default!;
}