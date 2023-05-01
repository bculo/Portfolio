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
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
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

        public TrendApiFactory()
        {

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.TokenValidationParameters.ValidateLifetime = false;
                    opt.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    opt.TokenValidationParameters.ValidateAudience = false;
                    opt.TokenValidationParameters.ValidateIssuer = false;
                });
                
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
}
