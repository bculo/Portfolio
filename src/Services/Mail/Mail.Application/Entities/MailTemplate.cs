using Amazon.DynamoDBv2.DataModel;

namespace Mail.Application.Entities;

[DynamoDBTable("MailTemplate")]
public class MailTemplate 
{
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
    
    [DynamoDBRangeKey("Name")]
    public string Name { get; set; }
    
    [DynamoDBHashKey("Category")]
    public int Category { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Template { get; set; } = default!;
    public string TemplateResolver { get; set; } = default!;
    public string ModifiedBy { get; set; } = default!;
    public DateTime ModificationDate { get; set; }
}