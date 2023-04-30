using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Trend.IntegrationTests
{
    public interface IApiFactory
    {
        HttpClient Client { get; }
    }

    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime, IApiFactory
    {
        private readonly MongoDbTestcontainer _sqlServerContainer = new TestcontainersBuilder<MongoDbTestcontainer>()
            .WithName($"Crypto.API.Integration.{Guid.NewGuid()}")
            .Build();

        public HttpClient Client { get; private set; }

        public TrendApiFactory()
        {

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {

            Client = CreateClient();
        }

        public async new Task DisposeAsync()
        {
        }       
    }

    internal class TestcontainersBuilder<T>
    {
        public TestcontainersBuilder()
        {
        }
    }
}
