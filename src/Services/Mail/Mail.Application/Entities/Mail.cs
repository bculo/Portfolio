using Amazon.DynamoDBv2.DataModel;

namespace Mail.Application.Entities;

[DynamoDBTable("Mail")]
public class Mail
{
    public string Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
    public string UserId { get; set; }
}