using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Trend.Application.Repositories;
using Trend.Application.Utils.Persistence;
using Trend.Domain.Entities;

namespace Trend.IntegrationTests;

public class TrendFixtureService
{
    private IServiceProvider Provider { get; set; }

    public TrendFixtureService(IServiceProvider provider)
    {
        Provider = provider;
    }

    public async Task AddSearchWord(SearchWord word)
    {
        var mongoClient = Provider.GetRequiredService<IMongoClient>();
        var collection = mongoClient.GetDatabase(TrendConstantsTest.DB_NAME).GetCollection<SearchWord>(TrendMongoUtils.GetCollectionName(nameof(SearchWord)));
        await collection.InsertOneAsync(word, new InsertOneOptions{}, default);
    }

    public async Task DropDatabase()
    {
        var mongoClient = Provider.GetRequiredService<IMongoClient>();
        await mongoClient.DropDatabaseAsync(TrendConstantsTest.DB_NAME);
    }
}