using DotNet.Testcontainers.Containers;
using Testcontainers.RabbitMq;
using Tests.Common.Interfaces.Containers;

namespace Tests.Common.Services.Containers;

public class RabbitMqFixture : IContainerFixture
{
    private readonly RabbitMqContainer _container = new RabbitMqBuilder()
        .WithImage("masstransit/rabbitmq")
        .WithName($"RabbitMQ.{Guid.NewGuid()}")
        .Build();

    public async Task InitializeAsync()
    {
        await StartAsync();
    }

    public async Task DisposeAsync()
    {
        await StopAsync();
    }

    public async Task StartAsync()
    {
        await _container.StartAsync();
    }

    public async Task StopAsync()
    {
        await _container.StopAsync();
    }

    public string GetConnectionString()
    {
        return _container.GetConnectionString();
    }
}