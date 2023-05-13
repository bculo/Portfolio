using Amazon.DynamoDBv2.DataModel;

namespace Mail.Application.Entities;

[DynamoDBTable("MailTemplate")]
public class MailTemplate
{
    [DynamoDBHashKey("Id")]
    public string Id { get; set; }
    [DynamoDBProperty("Name")]
    public string Name { get; set; }
    [DynamoDBProperty("Content")]
    public string Content { get; set; }
    [DynamoDBProperty("Created")]
    public DateTime Created { get; set; }
}