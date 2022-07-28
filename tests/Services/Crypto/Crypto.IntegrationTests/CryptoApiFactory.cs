using Crypto.Infrastracture.Persistence;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
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
        private readonly MsSqlTestcontainer _sqlServerContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration()
            {
                Password = "yourStrong(!)Password"
            })
            .WithName($"Crypto.API.Integration.{Guid.NewGuid()}")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(async services =>
            {
                await ConfigureDatabase(services);
            });
        }

        public async Task InitializeAsync()
        {
            await Task.WhenAll(_sqlServerContainer.StartAsync());
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await Task.WhenAll(_sqlServerContainer.StopAsync());
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
            await CryptoDbContextSeed.SeedData(services);
        }
    }
}
