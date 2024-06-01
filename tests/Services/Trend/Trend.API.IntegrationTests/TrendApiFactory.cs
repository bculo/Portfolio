using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims;
using Tests.Common.Interfaces.Containers;
using Tests.Common.Services.AuthHandlers;
using Tests.Common.Services.Claims;
using Tests.Common.Services.Containers;
using WireMock.Server;

namespace Trend.IntegrationTests
{
    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IContainerFixture _redisContainer = new RedisFixture();
        private readonly IContainerFixture _mqContainer = new RabbitMqFixture();
        private readonly IContainerFixture _azuriteContainer = new AzureBlobStorageFixture();
        private readonly IContainerFixture _mongoDbContainer = new MongoFixture(new MongoCredentials("mongo", "mongo"));

        public HttpClient Client { get; private set; } = default!;
        public WireMockServer MockServer { get; private set; } = default!;
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((_, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
            });
            
            var configurationValues = new Dictionary<string, string>
            {
                { "MongoOptions:ServerType", "0"},
                { "MongoOptions:DatabaseName", TrendConstantsTest.DbName },
                { "MongoOptions:ConnectionString", _mongoDbContainer.GetConnectionString() },
                { "GoogleSearchOptions:Uri", MockServer.Urls[0] },
                { "QueueOptions:Address", _mqContainer.GetConnectionString() },
                { "RedisOptions:ConnectionString", _redisContainer.GetConnectionString() },
                { "RedisOptions:InstanceName", TrendConstantsTest.RedisName },
                { "BlobStorageOptions:ConnectionString", _azuriteContainer.GetConnectionString() }
            };
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationValues!)
                .Build();

            builder.UseConfiguration(configuration);
            builder.ConfigureTestServices(services =>
            {
                services.AddDefaultFakeAuth();
                services.AddScoped<TrendFixtureService>();
            });
        }
        
        public async Task InitializeAsync()
        {
            await Task.WhenAll(_mongoDbContainer.StartAsync(), 
                _redisContainer.StartAsync(), 
                _mqContainer.StartAsync(),
                _azuriteContainer.StartAsync());
            
            MockServer = WireMockServer.Start();
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
}
