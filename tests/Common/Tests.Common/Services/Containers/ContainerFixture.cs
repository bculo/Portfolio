using DotNet.Testcontainers.Containers;
using Tests.Common.Interfaces.Containers;

namespace Tests.Common.Services.Containers;

public abstract class ContainerFixture : IContainerFixture
{
    protected abstract DockerContainer Container { get; }
    
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
        await Container.StartAsync();
    }

    public async Task StopAsync()
    {
        await Container.StopAsync();
    }

    public abstract string GetConnectionString();
}