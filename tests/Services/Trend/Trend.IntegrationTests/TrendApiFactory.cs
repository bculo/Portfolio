using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Security.Claims;
using Testcontainers.MongoDb;
using Testcontainers.Redis;
using Tests.Common.Interfaces;
using Tests.Common.Services;
using Trend.Application.Configurations.Options;
using Trend.Application.Utils;
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
            .WithCleanUp(true)
            .Build();
        
        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:7.2")
            .WithName($"Trend.API.Integration.Redis.{Guid.NewGuid()}")
            .WithCleanUp(true)
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
            
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IMockClaimSeeder, MockClaimSeeder>();
                services.AddSingleton<IAuthenticationSchemeProvider, MockJwtSchemeProvider>();
                
                services.RemoveAll(typeof(IMongoClient));
                services.AddSingleton<IMongoClient>(_ =>
                {
                    var options = new MongoOptions
                    {
                        ConnectionString = _mongoDbContainer.GetConnectionString(),
                        UseInterceptor = false,
                    };
                    
                    return TrendMongoUtils.CreateMongoClient(options);
                });

                services.AddStackExchangeRedisOutputCache(opt =>
                {
                    opt.InstanceName = "TrendIntegrationTest";
                    opt.Configuration = _redisContainer.GetConnectionString();
                });

                services.AddScoped<TrendFixtureService>();
                
                //services.AddSingleton(MockServer);
            });
            
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MongoOptions:ServerType", "0"), // use standalone instance
                    new KeyValuePair<string, string>("MongoOptions:DatabaseName", TrendConstantsTest.DB_NAME),
                    new KeyValuePair<string, string>("MongoOptions:ConnectionString", _mongoDbContainer.GetConnectionString()),
                    new KeyValuePair<string, string>("GoogleSearchOptions:Uri", MockServer.Urls[0]),
                }!);
            });
        }
        
        public async Task InitializeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StartAsync(), _redisContainer.StartAsync());
            Client = CreateClient();
        }

        public new async Task DisposeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StopAsync(), _redisContainer.StopAsync());
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
