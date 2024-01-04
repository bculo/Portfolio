using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;

namespace Trend.Application.Repositories;

public class MongoAuditableRepository<TEntity> : MongoRepository<TEntity>, IMongoAuditableRepository<TEntity> 
    where TEntity : AuditableDocument
{
    public MongoAuditableRepository(IMongoClient client, 
        IOptions<MongoOptions> options,
        IDateTimeProvider timeProvider) 
        : base(client, options, timeProvider)
    {
    }
    
    public Task<List<TEntity>> GetActiveItems(CancellationToken token)
    {
        var sortFilter = Builders<TEntity>.Sort.Descending(x => x.Created);
        
        var result = _collection.Find(i => i.IsActive)
            .Sort(sortFilter)
            .ToList();

        return Task.FromResult(result);
    }
    
    public async IAsyncEnumerable<TEntity> GetActiveItemsEnumerable([EnumeratorCancellation] CancellationToken token)
    {
        using var cursor = await _collection.Find(i => i.IsActive)
            .SortByDescending(i => i.Created)
            .ToCursorAsync(token);
        
        while (await cursor.MoveNextAsync(token))
        {
            foreach (var item in cursor.Current)
            {
                yield return item;
            }
        }
    }

    public async Task ActivateItems(IEnumerable<string> itemIds, CancellationToken token)
    {
        var update = Builders<TEntity>.Update.Set(s => s.IsActive, false)
            .Set(s => s.DeactivationDate, _timeProvider.Now);
            
        await _collection.UpdateManyAsync(i => itemIds.Contains(i.Id), update, new UpdateOptions(), token);
    }

    public async Task DeactivateItems(IEnumerable<string> itemIds, CancellationToken token)
    {
        var update = Builders<TEntity>.Update.Set(s => s.IsActive, false)
            .Set(s => s.DeactivationDate, _timeProvider.Now);
            
        await _collection.UpdateManyAsync(i => itemIds.Contains(i.Id), update, new UpdateOptions(), token);
    }
}