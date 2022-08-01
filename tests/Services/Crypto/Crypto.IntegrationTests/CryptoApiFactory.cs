using Crypto.Application.Interfaces.Services;
using Crypto.Infrastracture.Persistence;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using Crypto.Mock.Common.Clients;
using Crypto.Mock.Common.Data;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Crypto.IntegrationTests
{
    public class CryptoApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly RabbitMqTestcontainer _rabbitMqContainer = new TestcontainersBuilder<RabbitMqTestcontainer>()
            .WithMessageBroker(new RabbitMqTestcontainerConfiguration
            {
                Username = "rabbitmqusertest",
                Password = "rabbitmqpasswordtest"
            })
            .WithName($"RabbitMq.Test.Integration.{Guid.NewGuid()}")
            .Build();

        private readonly MsSqlTestcontainer _sqlServerContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration()
            {
                Password = "yourStrong(!)Password"
            })
            .WithName($"Crypto.API.Integration.{Guid.NewGuid()}")
            .Build();

        public readonly CryptoDataManager _seeder;

        public CryptoApiFactory()
        {
            _seeder = new CryptoDataManager();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                ConfigureDatabase(services).Wait();
                ConfigureServices(services);
                ConfigureRabbitMq(services);
            });
        }

        public async Task InitializeAsync()
        {
            await Task.WhenAll(_sqlServerContainer.StartAsync(),
                _rabbitMqContainer.StartAsync());
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await Task.WhenAll(_sqlServerContainer.StopAsync(),
                _rabbitMqContainer.StopAsync());
        }

        private async Task ConfigureDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CryptoDbContext>));

            services.Remove(descriptor);
            services.RemoveAll(typeof(CryptoDbContext));

            var connectionString = SqlServerUtils.ChangeConnectionDatabaseName(_sqlServerContainer.ConnectionString, "CryptoIntegrationDb");

            services.AddDbContext<CryptoDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            services.Migrate<CryptoDbContext>();
            await CryptoDbContextSeed.SeedData(services, _seeder.GetCryptoSeedData);
        }

        private void ConfigureRabbitMq(IServiceCollection services)
        {
            var massTransitDescriptors = services.Where(d => d.ServiceType.Namespace!.Contains("MassTransit", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in massTransitDescriptors)
            {
                services.Remove(item);
            }

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(_rabbitMqContainer.ConnectionString);
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //Configure ICryptoInfoService 
            services.RemoveAll(typeof(ICryptoInfoService));
            services.AddScoped<ICryptoInfoService>((provider) => new MockCryptoInfoService(_seeder.GetSupportedCryptoSymbolsArray()));

            //Configure ICryptoPriceService 
            services.RemoveAll(typeof(ICryptoPriceService));
            services.AddScoped<ICryptoPriceService>((provider) => new MockCryptoPriceService(_seeder.GetSupportedCryptoSymbolsArray()));
        }
    }
}
