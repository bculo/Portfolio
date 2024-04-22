using Amazon.DynamoDBv2.DataModel;
using Mail.Application.Interfaces;
using Mail.Application.Interfaces.Repository;

namespace Mail.Infrastructure.Repository;

public class MailRepository : IMailRepository
{
    private readonly IDynamoDBContext _dbContext;

    public MailRepository(IDynamoDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddItem(Application.Entities.Mail mail, CancellationToken token)
    {
        await _dbContext.SaveAsync(mail, token);
    }

    public async Task<Application.Entities.Mail> GetSingle(string userId, string id, CancellationToken token)
    {
        return await _dbContext.LoadAsync<Application.Entities.Mail>(userId, id, token);
    }
    
    public async Task<IEnumerable<Application.Entities.Mail>> GetAllUserMails(string userId, CancellationToken token)
    {
        return await _dbContext.QueryAsync<Application.Entities.Mail>(userId).GetRemainingAsync(token);
    }
}