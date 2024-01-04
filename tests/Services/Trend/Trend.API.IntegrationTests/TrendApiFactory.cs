using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Testcontainers.Azurite;
using Testcontainers.MongoDb;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;
using Tests.Common.Interfaces;
using Tests.Common.Services;
using WireMock.Server;

namespace Trend.IntegrationTests
{
    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:7.0.4")
            .WithUsername("mongo")
            .WithPassword("mongo")
            .WithName($"Trend.API.Integration.Mongo.{Guid.NewGuid()}")
            .Build();
        
        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:7.2")
            .WithName($"Trend.API.Integration.Redis.{Guid.NewGuid()}")
            .Build();

        private readonly AzuriteContainer _azuriteContainer = new AzuriteBuilder()
            .WithImage("mcr.microsoft.com/azure-storage/azurite")
            .WithExposedPort(10010)
            .WithName($"Trend.API.Integration.Azurite.{Guid.NewGuid()}")
            .Build();
        
        private readonly RabbitMqContainer _mqContainer = new RabbitMqBuilder()
            .WithImage("masstransit/rabbitmq")
            .WithName($"Trend.API.Integration.RabbitMQ.{Guid.NewGuid()}")
            .Build();
        
        public HttpClient Client { get; private set; }
        
        public WireMockServer MockServer { get; private set; }

        public TrendApiFactory()
        {
            MockServer = WireMockServer.Start();
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((_, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
            });
            
            var configurationValues = new Dictionary<string, string>
            {
                { "MongoOptions:ServerType", "0"},
                { "MongoOptions:DatabaseName", TrendConstantsTest.DB_NAME },
                { "MongoOptions:ConnectionString", _mongoDbContainer.GetConnectionString() },
                { "GoogleSearchOptions:Uri", MockServer.Urls[0] },
                { "QueueOptions:Address", _mqContainer.GetConnectionString() },
                { "RedisOptions:ConnectionString", _redisContainer.GetConnectionString() },
                { "RedisOptions:InstanceName", TrendConstantsTest.REDIS_NAME },
                { "BlobStorageOptions:ConnectionString", _azuriteContainer.GetConnectionString() }
            };
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationValues!)
                .Build();

            builder.UseConfiguration(configuration);
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IMockClaimSeeder, MockClaimSeeder>();
                services.AddSingleton<IAuthenticationSchemeProvider, MockJwtSchemeProvider>();
                services.AddScoped<TrendFixtureService>();
            });
        }
        
        public async Task InitializeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StartAsync(), 
                _redisContainer.StartAsync(), 
                _mqContainer.StartAsync(),
                _azuriteContainer.StartAsync());
            Client = CreateClient();
        }

        public new async Task DisposeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StopAsync(), 
                _redisContainer.StopAsync(), 
                _mqContainer.StopAsync(),
                _azuriteContainer.StopAsync());
            MockServer.Stop();
        }       
    }
    

    public class MockClaimSeeder : IMockClaimSeeder
    {
        private Dictionary<UserAuthType, List<Claim>> _claimDict;

        public MockClaimSeeder()
        {
            _claimDict = new Dictionary<UserAuthType, List<Claim>>
            {
                { UserAuthType.None, new List<Claim>() },      
                { UserAuthType.User, new List<Claim>() }
            };
        }
        
        public IEnumerable<Claim> GetClaims(int userTypeId)
        {
            if (!Enum.GetValues<UserAuthType>().Cast<int>().Contains(userTypeId))
            {
                throw new Exception("Given userTypeId is not enum");
            }
            
            return _claimDict[(UserAuthType)userTypeId];
        }
    }
}
