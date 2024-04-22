using Mail.Application.Entities;
using Mail.Application.Entities.Enums;

namespace Mail.Application.Interfaces.Repository;

public interface IMailTemplateRepository
{
    Task<IEnumerable<MailTemplate>> GetAll(CancellationToken ct);
    Task<MailTemplate> GetSingle(MailTemplateCategory category, string template, CancellationToken ct);
    Task<IEnumerable<MailTemplate>> GetTemplatesByCategory(MailTemplateCategory category, 
        bool isActive, 
        CancellationToken ct);
    Task AddItem(MailTemplate template, CancellationToken ct);
    Task UpdateItem(MailTemplate template, CancellationToken ct);
}