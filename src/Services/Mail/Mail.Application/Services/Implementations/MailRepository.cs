using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Mail.Application.Services.Interfaces;

namespace Mail.Application.Services.Implementations;

public class MailRepository : IMailRepository
{
    private readonly IDynamoDBContext _dbContext;
    private readonly IAmazonDynamoDB _client;
    
    public MailRepository(IDynamoDBContext dbContext, IAmazonDynamoDB client)
    {
        _dbContext = dbContext;
        _client = client;
    }
    
    public async Task AddItem(Entities.Mail mail)
    {
        await _dbContext.SaveAsync(mail);
    }

    public async Task<Entities.Mail> GetSingle(string userId, string id)
    {
        return await _dbContext.LoadAsync<Entities.Mail>(userId, id);
    }
    
    public async Task<IEnumerable<Entities.Mail>> GetAllUserMails(string userId)
    {
        return await _dbContext.QueryAsync<Entities.Mail>(userId).GetRemainingAsync();
    }
}