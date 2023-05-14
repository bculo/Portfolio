using Mail.Application.Entities;

namespace Mail.Application.Models;

public class MailTemplateDetailsDto
{
    public int Category { get; set; }
    public string TemplateName { get; set; }
    public string Template { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModificationDate { get; set; }
    public bool IsActive { get; set; }
}

public static class MailTemplateDetailsDtoMapper
{
    public static MailTemplateDetailsDto ToDetailsDto(this MailTemplate template)
    {
        ArgumentNullException.ThrowIfNull(template, nameof(template));
        
        return new MailTemplateDetailsDto
        {
            Category = template.Category,
            Template = template.Template,
            TemplateName = template.Name,
            Title = template.Title,
            Created = template.Created,
            IsActive = template.IsActive,
            ModificationDate = template.ModificationDate,
            ModifiedBy = template.ModifiedBy
        };
    }
}