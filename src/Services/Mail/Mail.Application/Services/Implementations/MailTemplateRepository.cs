using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Mail.Application.Entities;
using Mail.Application.Entities.Enums;
using Mail.Application.Models;
using Mail.Application.Services.Interfaces;

namespace Mail.Application.Services.Implementations;

public class MailTemplateRepository : IMailTemplateRepository
{
    private readonly IDynamoDBContext _dbContext;
    private readonly IAmazonDynamoDB _client;
    
    public MailTemplateRepository(
        IDynamoDBContext dbContext,
        IAmazonDynamoDB client)
    {
        _dbContext = dbContext;
        _client = client;
    }

    public async Task<IEnumerable<MailTemplate>> GetAll(CancellationToken ct)
    {
        return await _dbContext.ScanAsync<MailTemplate>(null).GetRemainingAsync(ct);
    }
    
    public async Task<MailTemplate> GetSingle(MailTemplateCategory category, string template, CancellationToken ct)
    {
        var itemRequest = new GetItemRequest()
        {
            TableName = nameof(MailTemplate),
            Key = new Dictionary<string, AttributeValue>()
            {
                { "Category", new AttributeValue { N = ((int)category).ToString() } },
                { "Name", new AttributeValue { S = template } }
            },
        };

        var response = await _client.GetItemAsync(itemRequest, ct);
        if (response.Item.Count == 0)
        {
            return null;
        }
        
        var doc = Document.FromAttributeMap(response.Item);
        return _dbContext.FromDocument<MailTemplate>(doc);
    }

    public async Task<IEnumerable<MailTemplate>> GetTemplatesByCategory(MailTemplateCategory category, 
        bool isActive, CancellationToken ct)
    {
        var query = new QueryRequest()
        {
            TableName = nameof(MailTemplate),
            KeyConditionExpression = "Category = :categoryId",
            FilterExpression = "IsActive = :isActive",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                { ":categoryId", new AttributeValue() { N = ((int)category).ToString() } },
                { ":isActive", new AttributeValue() { N = (isActive) ? "1" : "0" } },
            }
        };

        var response = await _client.QueryAsync(query, ct);
        if (response.Items.Count == 0)
        {
            return Enumerable.Empty<MailTemplate>();
        }

        return response.Items.Select(i =>
        {
            var doc = Document.FromAttributeMap(i);
            return _dbContext.FromDocument<MailTemplate>(doc);
        });
    }

    public async Task AddItem(MailTemplate template, CancellationToken ct)
    {
        await _dbContext.SaveAsync(template, ct);
    }

    public async Task UpdateItem(MailTemplate template, CancellationToken ct)
    {
        await _dbContext.SaveAsync(template, ct);
    }
}