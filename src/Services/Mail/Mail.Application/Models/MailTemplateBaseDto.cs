using Mail.Application.Entities;

namespace Mail.Application.Models;

public class MailTemplateBaseDto
{
    public int Category { get; set; }
    public string TemplateName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
}

public static class MailTemplateBaseDtoMapper
{
    public static MailTemplateBaseDto ToBaseDto(this MailTemplate template)
    {
        ArgumentNullException.ThrowIfNull(template, nameof(template));
        
        return new MailTemplateBaseDto
        {
            Category = template.Category,
            TemplateName = template.Name,
            Title = template.Title,
            Created = template.Created,
            IsActive = template.IsActive
        };
    }
}