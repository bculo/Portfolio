using Mail.Application.Entities;
using Mail.Application.Entities.Enums;

namespace Mail.Application.Services.Interfaces;

public interface IMailTemplateRepository
{
    Task<IEnumerable<MailTemplate>> GetAll();
    Task<MailTemplate> GetSingle(MailTemplateCategory category, string template);
    Task<IEnumerable<MailTemplate>> GetTemplatesByCategory(MailTemplateCategory category, bool isActive);
    Task AddItem(MailTemplate template);
    Task UpdateItem(MailTemplate template);
}