using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Trend.Application.Interfaces;
using Trend.Application.Utils;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.IntegrationTests;

public class TrendFixtureService(IServiceProvider provider)
{
    private static readonly Fixture Fixture = new();
    
    private readonly IMongoClient _client = provider.GetRequiredService<IMongoClient>();
    private readonly ITransaction _transaction = provider.GetRequiredService<ITransaction>();

    private T GenerateWithMongoId<T>() where T : RootDocument
    {
        return Fixture.Build<T>()
                    .With(x => x.Id, TrendMongoUtils.GenerateId())
                    .Create();
    }

    public static T GenerateMockInstance<T>() where T : class
    {
        return Fixture.Create<T>();
    }
    
    public async Task<Domain.Entities.SearchWord> AddSearchWord(
        string? id = null, 
        string? searchWord = null,
        SearchEngine? engine = default,
        ContextType? context = default)
    {
        var instance = GenerateWithMongoId<Domain.Entities.SearchWord>();
        instance.Engine = engine ?? SearchEngine.Google;
        instance.Type = context ?? ContextType.Stock;
        instance.Id = id ?? instance.Id;
        instance.Id = searchWord ?? instance.Word;
        
        var collection = _client.GetDatabase(TrendConstantsTest.DbName)
            .GetCollection<Domain.Entities.SearchWord>(TrendMongoUtils.GetCollectionName(nameof(SearchWord)));
        await collection.InsertOneAsync(instance, new InsertOneOptions{}, default);
        await _transaction.CommitTransaction();
        
        return instance;
    }
    
    public async Task<Domain.Entities.SearchWord> AddSearchWord(Domain.Entities.SearchWord instance)
    {
        var collection = _client.GetDatabase(TrendConstantsTest.DbName)
            .GetCollection<Domain.Entities.SearchWord>(TrendMongoUtils.GetCollectionName(nameof(SearchWord)));
        await collection.InsertOneAsync(instance, new InsertOneOptions{}, default);
        await _transaction.CommitTransaction();
        return instance;
    }
    
    public async Task<SyncStatus> AddSyncStatus()
    {
        var syncStatus = Fixture.Build<SyncStatus>()
            .With(x => x.Id, ObjectId.GenerateNewId().ToString())
            .With(x => x.Created, DateTime.Now.AddHours(-2))
            .With(x => x.Finished, DateTime.Now.AddHours(-2))
            .Create();

        syncStatus.UsedSyncWords = [new SyncStatusWord() { Type = ContextType.Stock, WordId = "123" }];
        
        var collection = _client.GetDatabase(TrendConstantsTest.DbName)
            .GetCollection<SyncStatus>(TrendMongoUtils.GetCollectionName(nameof(SyncStatus)));
        await collection.InsertOneAsync(syncStatus, new InsertOneOptions{});
        await _transaction.CommitTransaction();
        return syncStatus;
    }

    public async Task DropDatabase()
    {
        await _client.DropDatabaseAsync(TrendConstantsTest.DbName);
    }
}