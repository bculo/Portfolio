using Crypto.Application.Interfaces.Services;
using Crypto.Infrastracture.Consumers;
using Crypto.Infrastracture.Persistence;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Interfaces;
using Crypto.Mock.Common.Clients;
using Crypto.Mock.Common.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using Testcontainers.MsSql;
using Testcontainers.Redis;
using WireMock.Server;

namespace Crypto.IntegrationTests
{
    public class CryptoApiFactory : WebApplicationFactory<Program>, IAsyncLifetime, IApiFactory
    {
        private readonly MsSqlContainer _sqlServerContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithName($"Crypto.API.Integration.MsSql.{Guid.NewGuid()}")
            .WithCleanUp(true)
            .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:7.0.2")
            .WithName($"Crypto.API.Integration.Redis.{Guid.NewGuid()}")
            .WithCleanUp(true)
            .Build();

        private Respawner _respawner = default!;
        private readonly ICryptoDataManager _dataManager;

        public WireMockServer InfoMockServer { get; private set; }
        public WireMockServer PriceMockServer { get; private set; }

        public HttpClient Client { get; private set; } = default!;

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_sqlServerContainer.GetConnectionString());
        }

        public CryptoApiFactory()
        {
            InfoMockServer = WireMockServer.Start();
            PriceMockServer = WireMockServer.Start();

            _dataManager = new DefaultDataManager();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configrationBuilder =>
            {
                configrationBuilder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("CryptoPriceApiOptions:BaseUrl", PriceMockServer.Url),
                    new KeyValuePair<string, string>("CryptoInfoApiOptions:BaseUrl", InfoMockServer.Url),
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

                ConfigureRedis(services);
                ConfigureDatabase(services).Wait();
                ConfigureServices(services);
                ConfigureRabbitMq(services);
            });
        }

        public async Task InitializeAsync()
        {
            await Task.WhenAll(_sqlServerContainer.StartAsync(), _redisContainer.StartAsync());
            _respawner = await InitializeRespawner();
            Client = CreateClient();
        }

        public new async Task DisposeAsync()
        {
            await Task.WhenAll(_sqlServerContainer.StopAsync(), _redisContainer.StopAsync());

            InfoMockServer.Stop();
            PriceMockServer.Stop();
        }

        private async Task<Respawner> InitializeRespawner()
        {
            return await Respawner.CreateAsync(_sqlServerContainer.GetConnectionString(), 
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.SqlServer
                });
        }

        private async Task ConfigureDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CryptoDbContext>));

            services.Remove(descriptor);
            services.RemoveAll(typeof(CryptoDbContext));

            services.AddDbContext<CryptoDbContext>(opt =>
            {
                opt.UseSqlServer(_sqlServerContainer.GetConnectionString());
            });

            services.Migrate<CryptoDbContext>();
            await CryptoDbContextSeed.SeedData(services, _dataManager.GetCryptoSeedData);
        }

        private void ConfigureRedis(IServiceCollection services)
        {
            services.RemoveAll(typeof(IDistributedCache));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
                options.InstanceName = "RedisIntegrationTest";
            });
        }

        private void ConfigureRabbitMq(IServiceCollection services)
        {
            services.AddMassTransitTestHarness(x =>
            {
                x.AddInMemoryInboxOutbox();

                x.AddDelayedMessageScheduler();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Crypto", false));

                x.AddConsumer<AddCryptoItemConsumer>();
                x.AddConsumer<CryptoVisitedConsumer>();

                x.UsingInMemory((context, config) =>
                {
                    config.UseDelayedMessageScheduler();
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.RemoveAll(typeof(ICryptoInfoService));
            services.AddScoped<ICryptoInfoService>((provider) => new MockCryptoInfoService(_dataManager.GetSupportedCryptoSymbolsArray()));

            //Configure ICryptoPriceService 
            services.RemoveAll(typeof(ICryptoPriceService));
            services.AddScoped<ICryptoPriceService>((provider) => new MockCryptoPriceService(_dataManager.GetSupportedCryptoSymbolsArray()));
        }
    }
}
