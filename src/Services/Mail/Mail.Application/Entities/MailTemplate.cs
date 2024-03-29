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
    public int Category { get; set; }
    public string Title { get; set; }
    public string Template { get; set; }
    public string TemplateResolver { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModificationDate { get; set; }
}