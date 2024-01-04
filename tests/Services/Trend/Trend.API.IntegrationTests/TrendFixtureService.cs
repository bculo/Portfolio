using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;
using Trend.Application.Repositories;
using Trend.Application.Utils;
using Trend.Domain.Entities;

namespace Trend.IntegrationTests;

public class TrendFixtureService
{
    private static readonly Fixture _fixture = new();

    private readonly IServiceProvider _provider;
    private readonly IMongoClient _client;

    public TrendFixtureService(IServiceProvider provider)
    {
        _provider = provider;
        _client = _provider.GetRequiredService<IMongoClient>();
    }

    private T GenerateWithMongoId<T>() where T : RootDocument
    {
        return _fixture.Build<T>()
                    .With(x => x.Id, TrendMongoUtils.GenerateId())
                    .Create();
    }

    public static T GenerateMockInstance<T>() where T : class
    {
        return _fixture.Create<T>();
    }
    
    public async Task<SearchWord> AddSearchWord(string? id = null, string? searchWord = null)
    {
        var instance = GenerateWithMongoId<SearchWord>();
        
        if(id is not null)
        {
            instance.Id = id;
        }
        
        if(searchWord is not null)
        {
            instance.Word = searchWord;
        }
        
        var collection = _client.GetDatabase(TrendConstantsTest.DB_NAME)
            .GetCollection<SearchWord>(TrendMongoUtils.GetCollectionName(nameof(SearchWord)));
        await collection.InsertOneAsync(instance, new InsertOneOptions{}, default);
        return instance;
    }
    
    public async Task<SearchWord> AddSearchWord(SearchWord instance)
    {
        var collection = _client.GetDatabase(TrendConstantsTest.DB_NAME)
            .GetCollection<SearchWord>(TrendMongoUtils.GetCollectionName(nameof(SearchWord)));
        await collection.InsertOneAsync(instance, new InsertOneOptions{}, default);
        return instance;
    }
    
    public async Task<SyncStatus> AddSyncStatus()
    {
        var syncStatus = _fixture.Build<SyncStatus>()
            .With(x => x.Id, ObjectId.GenerateNewId().ToString())
            .With(x => x.Created, DateTime.Now.AddHours(-2))
            .With(x => x.Finished, DateTime.Now.AddHours(-2))
            .Create();
        var collection = _client.GetDatabase(TrendConstantsTest.DB_NAME).GetCollection<SyncStatus>(TrendMongoUtils.GetCollectionName(nameof(SyncStatus)));
        await collection.InsertOneAsync(syncStatus, new InsertOneOptions{}, default);
        return syncStatus;
    }

    public async Task DropDatabase()
    {
        await _client.DropDatabaseAsync(TrendConstantsTest.DB_NAME);
    }
}