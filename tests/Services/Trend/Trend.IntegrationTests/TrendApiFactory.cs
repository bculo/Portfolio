using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tests.Common.Interfaces;
using Tests.Common.Services;
using Trend.Application.Options;
using Trend.Application.Utils.Persistence;

namespace Trend.IntegrationTests
{
    public interface IApiFactory
    {
        HttpClient Client { get; }
    }

    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime, IApiFactory
    {
        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:latest")
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

                    return TrendMongoUtils.CreateMongoClient(options);
                });
            });
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
