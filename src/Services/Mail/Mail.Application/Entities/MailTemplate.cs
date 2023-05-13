using Amazon.DynamoDBv2.DataModel;

namespace Mail.Application.Entities;

public class MailTemplate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Template { get; set; }
    public string TemplateResolver { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModificationDate { get; set; }
}