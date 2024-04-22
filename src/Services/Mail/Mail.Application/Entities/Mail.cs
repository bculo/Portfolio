using Amazon.DynamoDBv2.DataModel;

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
    public bool IsActive { get; set; }
    public string UserId { get; set; } = default!;
}