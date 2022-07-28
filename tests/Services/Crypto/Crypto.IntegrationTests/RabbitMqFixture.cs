using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.IntegrationTests
{
    public class RabbitMqFixture : IDisposable
    {
        private readonly RabbitMqTestcontainer _rabbitMqContainer = new TestcontainersBuilder<RabbitMqTestcontainer>()
            .WithMessageBroker(new RabbitMqTestcontainerConfiguration
            {
                Username = "rabbitmqusertest",
                Password = "rabbitmqpasswordtest"
            })
            .WithName($"RabbitMq.Test.Integration.{Guid.NewGuid()}")
            .Build();

        public void ConfigureRabbitMq(CryptoApiFactory factory)
        {
            factory.WithWebHostBuilder(conf =>
            {
                conf.ConfigureServices(services =>
                {
                    ConfigureRabbitMq(services);
                });
            });

            if (_rabbitMqContainer.State == TestcontainersState.Running)
                return;

            _rabbitMqContainer.StartAsync().Wait();
        }

        public void Dispose()
        {
            _rabbitMqContainer.StopAsync().Wait();
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
    }
}
