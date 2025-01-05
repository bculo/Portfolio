using DotNet.Testcontainers.Containers;
using Testcontainers.Redis;
using Tests.Common.Interfaces.Containers;

namespace Tests.Common.Services.Containers;

public class RedisFixture : IContainerFixture
{
    private readonly RedisContainer _container = new RedisBuilder()
        .WithImage("redis:7.2")
        .WithName($"Redis.{Guid.NewGuid()}")
        .WithCleanUp(true)
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