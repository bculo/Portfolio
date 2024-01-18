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
        IDateTimeProvider timeProvider,
        IClientSessionHandle clientSession) 
        : base(client, options, timeProvider, clientSession)
    {
    }
    
    public async Task<List<TEntity>> GetActiveItems(CancellationToken token = default)
    {
        var sortFilter = Builders<TEntity>.Sort.Descending(x => x.Created);
        
        return await Collection.Find(ClientSession, i => i.IsActive)
            .Sort(sortFilter)
            .ToListAsync(token);
    }

    public async Task<List<TEntity>> GetDeactivatedItems(CancellationToken token = default)
    {
        var sortFilter = Builders<TEntity>.Sort.Descending(x => x.Created);
        
        return await Collection.Find(ClientSession, i => !i.IsActive)
            .Sort(sortFilter)
            .ToListAsync(token);
    }

    public async IAsyncEnumerable<TEntity> GetActiveItemsEnumerable([EnumeratorCancellation] CancellationToken token = default)
    {
        using var cursor = await Collection.Find(ClientSession, i => i.IsActive)
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

    public async IAsyncEnumerable<TEntity> GetDeactivatedItemsEnumerable([EnumeratorCancellation] CancellationToken token = default)
    {
        using var cursor = await Collection.Find(i => !i.IsActive)
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

    public async Task ActivateItems(IEnumerable<string> itemIds, CancellationToken token = default)
    {
        var update = Builders<TEntity>.Update.Set(s => s.IsActive, true)
            .Set(s => s.DeactivationDate, TimeProvider.Now);
            
        await Collection.UpdateManyAsync(ClientSession, i => itemIds.Contains(i.Id), update, new UpdateOptions(), token);
    }

    public async Task DeactivateItems(IEnumerable<string> itemIds, CancellationToken token = default)
    {
        var update = Builders<TEntity>.Update.Set(s => s.IsActive, false)
            .Set(s => s.DeactivationDate, TimeProvider.Now);
            
        await Collection.UpdateManyAsync(ClientSession, i => itemIds.Contains(i.Id), update, new UpdateOptions(), token);
    }
}