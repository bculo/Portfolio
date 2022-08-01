
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trend.IntegrationTests.Utils;

namespace Trend.IntegrationTests
{
    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime, IDisposable
    {
        private readonly IDockerNetwork _network = new TestcontainersNetworkBuilder()
            .WithName($"Trend.Network.{Guid.NewGuid()}")
            .Build();

        private MsSqlTestcontainer? _sqlServerContainer;
        private readonly ITestcontainersBuilder<MsSqlTestcontainer>? _sqlServerBuilder;

        private TestcontainersContainer? _keycloakContainer;
        private readonly ITestcontainersBuilder<TestcontainersContainer>? _keycloakBuilder;

        public TrendApiFactory()
        {
            _sqlServerBuilder = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration()
                {
                    Password = "pa55w0rd!"
                })
                .WithNetwork(_network)
                .WithName($"Keycloak.Trend.API.Database");

            _keycloakBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("jboss/keycloak")
                .WithPortBinding(8081, 8080)
                .WithEnvironment("DB_VENDOR", "mssql")
                .WithEnvironment("DB_USER", "sa")
                .WithEnvironment("DB_ADDR", "Keycloak.Trend.API.Database")
                .WithEnvironment("DB_PASSWORD", "pa55w0rd!")
                .WithEnvironment("DB_DATABASE", "Keycloak")
                .WithEnvironment("KEYCLOAK_USER", "admin")
                .WithEnvironment("KEYCLOAK_PASSWORD", "admin")
                .WithNetwork(_network)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .WithName($"Keycloak.Trend.API.AuthorizationServer");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {
            await _network.CreateAsync();

            //Configure sql server
            _sqlServerContainer = _sqlServerBuilder!.Build();
            await _sqlServerContainer.StartAsync();
            CreateKeycloakDatabase();

            _keycloakContainer = _keycloakBuilder!.Build();
            await _keycloakContainer.StartAsync();
        }

        private void CreateKeycloakDatabase()
        {
            using var connection = new SqlConnection(_sqlServerContainer!.ConnectionString);
            connection.Open();
            using var cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "CREATE DATABASE Keycloak";
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _sqlServerContainer!.StopAsync();
            await _keycloakContainer!.StopAsync();
        }       
    }
}
