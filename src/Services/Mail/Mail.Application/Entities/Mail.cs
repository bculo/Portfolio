using Amazon.DynamoDBv2.DataModel;
using Mail.Application.Entities.Enums;

namespace Mail.Application.Entities;

[DynamoDBTable("Mail")]
public class Mail
{
    public string Id { get; set; } = default!;
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public DateTime Created { get; set; }
    public string UserId { get; set; } = default!;
    public Priority Priority { get; set; }
    public Sentiment Sentiment { get; set; }
    public Status Status { get; set; }
}