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
using Trend.Application.Options;
using Trend.Application.Utils.Persistence;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.IntegrationTests.SearchWordController;

namespace Trend.IntegrationTests
{
    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        
        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:latest")
            .WithUsername("mongo")
            .WithPassword("mongo")
            .WithName($"Trend.API.Integration.Mongo.{Guid.NewGuid()}")
            .Build();
        
        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:7.0.2")
            .WithName($"Trend.API.Integration.Redis.{Guid.NewGuid()}")
            .WithCleanUp(true)
            .Build();

        public HttpClient Client { get; private set; }
        
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
                    
                    _mongoClient = TrendMongoUtils.CreateMongoClient(options);
                    return _mongoClient;
                });

                services.AddStackExchangeRedisOutputCache(opt =>
                {
                    opt.InstanceName = "TrendIntegrationTest";
                    opt.Configuration = _redisContainer.GetConnectionString();
                });

                services.AddScoped<TrendFixtureService>();
            });
        }
        
        public async Task ResetDatabaseState()
        {
            await _mongoClient.DropDatabaseAsync("Trend");
        }
        
        public async Task InitializeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StartAsync(), _redisContainer.StartAsync());
            Client = CreateClient();
        }

        public new async Task DisposeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StopAsync(), _redisContainer.StopAsync());
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
