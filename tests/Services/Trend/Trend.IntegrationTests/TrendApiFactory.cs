using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Common.Interfaces;
using Tests.Common.Services;
using Time.Common.Contracts;
using Trend.Application.Options;
using Trend.Application.Utils.Persistence;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.IntegrationTests.SearchWordController;

namespace Trend.IntegrationTests
{
    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private string _mongoDbName;
        private IMongoClient _mongoClient;
        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:5.0")
            .WithUsername("testuser")
            .WithPassword("testuser")
            .WithName($"Trend.API.Integration.Mongo.{Guid.NewGuid()}")
            .Build();

        public HttpClient Client { get; private set; }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((context, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
            });
            
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IMockClaimSeeder, MockClaimSeeder>();
                services.AddSingleton<IAuthenticationSchemeProvider, MockJwtSchemeProvider>();
                
                services.RemoveAll(typeof(IMongoClient));
                services.AddSingleton<IMongoClient>(c =>
                {
                    var options = new MongoOptions
                    {
                        ConnectionString = _mongoDbContainer.GetConnectionString(),
                        UseInterceptor = false,
                    };

                    var configuration = c.GetRequiredService<IConfiguration>();
                    _mongoClient = TrendMongoUtils.CreateMongoClient(options);
                    _mongoDbName = configuration.GetValue<string>("MongoOptions:DatabaseName");
                    
                    SeedDatabase().GetAwaiter().GetResult();
                    
                    return _mongoClient;
                });
            });
        }
        
        public async Task ResetDatabaseState()
        {
            await _mongoClient.DropDatabaseAsync(_mongoDbName);
        }

        public Task SeedDatabase()
        {
            var db = _mongoClient.GetDatabase(_mongoDbName);
            var col = db.GetCollection<SearchWord>(nameof(SearchWord).ToLower());
            
            col.InsertOne(new SearchWord
            {
                Engine = SearchEngine.Google,
                Word = MockConstants.EXISTING_SEARCH_WORD_TEXT,
                Type = ContextType.Economy,
                IsActive = true,
                Id = MockConstants.EXISTING_SEARCH_WORD_ID,
            });
            
            return Task.CompletedTask;
        }
        
        public async Task InitializeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StartAsync());
            Client = CreateClient();
        }

        public async new Task DisposeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StopAsync());
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
