using Crypto.Application.Interfaces.Price;
using Crypto.Infrastructure.Persistence;
using Crypto.Infrastructure.Price;
using Cryptography.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Tests.Common;
using Tests.Common.Extensions;
using WireMock.Server;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.IntegrationTests;

public class CryptoApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient Client { get; private set; } = default!;
    public WireMockServer MockServer { get; private set; } = default!;

    private readonly string _redisConnectionString = "localhost:6379";
    private readonly string _redisInstanceName = $"cryptotest{Guid.NewGuid()}";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var keyPair = RsaUtils.GenerateKeyPair();
        
        var configuration = new Dictionary<string, string>
        {
            { "CryptoPriceApiOptions:BaseUrl", MockServer.Urls[0] },
            { "CryptoInfoApiOptions:BaseUrl", MockServer.Urls[0] },
            {
                "ConnectionStrings:CryptoDatabase",
                $"Host=localhost;Port=5433;Database=CryptoTst{Guid.NewGuid()};User Id=postgres;Password=florijan;"
            },
            { "QueueOptions:Address", "amqp://rabbitmquser:rabbitmqpassword@localhost:5672" },
            { "QueueOptions:Prefix", $"cryptotest{Guid.NewGuid()}" },
            { "QueueOptions:Temporary", "true" },
            { "RedisOptions:ConnectionString", _redisConnectionString },
            { "RedisOptions:InstanceName", _redisInstanceName },
            { "AuthOptions:PublicRsaKey", keyPair.PublicKey },
            { "AuthOptions:ValidIssuer", $"cryptotestissuer{Guid.NewGuid()}" },
            { "TestAuthentication:PrivateKeyBase64", keyPair.PrivateKey }
        }.AsConfiguration();

        builder.UseConfiguration(configuration);
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<ICryptoPriceService, MockPriceClient>();
            services.AddScoped<TestFixture>();
            services.AddTestAuthentication();
        });
    }

    public async Task InitializeAsync()
    {
        MockServer = WireMockServer.Start();
        Client = CreateClient();

        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
        await dbContext.Database.MigrateAsync();
        await dbContext.DisposeAsync();
    }

    public new async Task DisposeAsync()
    {
        MockServer.Stop();
        
        await using var scope = Services.CreateAsyncScope();

        await DeleteDatabaseAsync(scope);
        await CleanRedisCacheAsync(scope);
    }

    private async Task DeleteDatabaseAsync(AsyncServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.DisposeAsync();
    }
    
    private async Task CleanRedisCacheAsync(AsyncServiceScope scope)
    {
        var multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        var fusionCache = scope.ServiceProvider.GetRequiredService<IFusionCache>();
        var server = multiplexer.GetServer(_redisConnectionString);
        await foreach (var item in server.KeysAsync(pattern: $"*{_redisInstanceName}*"))
        {
            var splitter = ":";
            var keyParts = item.ToString().Split(splitter).ToList();
            keyParts.RemoveAt(0);
            await fusionCache.RemoveAsync(string.Join(splitter, keyParts));
        }
    }
}

