using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Mail.Application.Services.Interfaces;

namespace Mail.Application.Services.Implementations;

public class MailRepository : IMailRepository
{
    private readonly IDynamoDBContext _dbContext;

    public MailRepository(IDynamoDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddItem(Entities.Mail mail, CancellationToken token)
    {
        await _dbContext.SaveAsync(mail, token);
    }

    public async Task<Entities.Mail> GetSingle(string userId, string id, CancellationToken token)
    {
        return await _dbContext.LoadAsync<Entities.Mail>(userId, id, token);
    }
    
    public async Task<IEnumerable<Entities.Mail>> GetAllUserMails(string userId, CancellationToken token)
    {
        return await _dbContext.QueryAsync<Entities.Mail>(userId).GetRemainingAsync(token);
    }
}