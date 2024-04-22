using System.ComponentModel;

namespace Mail.Application.Services.Interfaces;

public interface IMailRepository
{
    Task AddItem(Entities.Mail mail, CancellationToken token);
    Task<Entities.Mail> GetSingle(string userId, string id, CancellationToken token);
    Task<IEnumerable<Entities.Mail>> GetAllUserMails(string userId, CancellationToken token);
    
}