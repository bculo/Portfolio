namespace Events.Common.Mail;

public class CheckMailSentiment
{
    public CheckMailSentimentBody RawMessage { get; set; } = default!;
    
    public CheckMailSentimentBody Body { get; set; } = default!;
    
    public Dictionary<string, string> Headers { get; set; } = new();
    
    public Dictionary<string, string> Path { get; set; } = new();
    
    public string ContentType { get; set; } = default!;
    
    public Guid MessageId { get; set; }
    
    public Guid CorrelationId { get; set; } = default!;
    
    public bool Processed { get; set; }
    
    public bool Commited { get; set; }
}

public class CheckMailSentimentBody
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string UserId { get; set; } = default!;
}