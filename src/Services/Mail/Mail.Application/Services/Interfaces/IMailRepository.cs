using System.ComponentModel;

namespace Mail.Application.Services.Interfaces;

public interface IMailRepository
{
    Task AddItem(Entities.Mail mail);
    Task<Entities.Mail> GetSingle(string userId, string id);
    
    Task<IEnumerable<Entities.Mail>> GetAllUserMails(string userId);
    
}