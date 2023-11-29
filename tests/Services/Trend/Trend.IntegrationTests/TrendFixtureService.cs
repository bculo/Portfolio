using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Trend.Application.Repositories;
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
        var collection = mongoClient.GetDatabase("Trend").GetCollection<SearchWord>(nameof(SearchWord).ToLower());
        await collection.InsertOneAsync(word, new InsertOneOptions{}, default);
    }
}