using Crypto.Infrastracture.Persistence;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
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
        /*
        private readonly TestcontainersContainer _dbContainer =
            new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SA_PASSWORD", "pa55w0rd!")
                .WithPortBinding(5777, 1433)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
        */

        private readonly MsSqlTestcontainer _sqlServerContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration()
                {
                    Password = "yourStrong(!)Password"
                })
                .WithName($"Crypto.API.Integration.{Guid.NewGuid()}")
                .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = SqlServerUtils.ChangeConnectionDatabaseName(_sqlServerContainer.ConnectionString, "CryptoIntegrationDb");

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(CryptoDbContext));

                services.AddDbContext<CryptoDbContext>(opt =>
                {
                    opt.UseSqlServer(connectionString);
                });

                services.Migrate<CryptoDbContext>();
            });
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();
        }
    }
}
