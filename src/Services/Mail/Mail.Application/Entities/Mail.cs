using Amazon.DynamoDBv2.DataModel;

namespace Mail.Application.Entities;

[DynamoDBTable("Mail")]
public class Mail : BaseEntity<string>
{
    public string From { get; set; }
    public string To { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}