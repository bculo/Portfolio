using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;
using Microsoft.Extensions.Configuration;

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
            .WithCleanUp(true)
            .Build();

        public HttpClient Client { get; private set; }

        public TrendApiFactory()
        {

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configrationBuilder =>
            {
                configrationBuilder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("SerilogMongo:UseLogger", "false"),
                    new KeyValuePair<string, string>("MongoOptions:UseInterceptor", "false"),
                    new KeyValuePair<string, string>("MongoOptions:ConnectionString", _mongoDbContainer.GetConnectionString()),
                });
            });

            builder.ConfigureTestServices(services =>
            {
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.TokenValidationParameters.ValidateLifetime = false;
                    opt.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    opt.TokenValidationParameters.ValidateAudience = false;
                    opt.TokenValidationParameters.ValidateIssuer = false;
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
}
