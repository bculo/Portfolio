using Testcontainers.Azurite;
using Tests.Common.Interfaces.Containers;

namespace Tests.Common.Services.Containers;

public class AzureBlobStorageFixture : IContainerFixture
{
    private readonly AzuriteContainer _container = new AzuriteBuilder()
        .WithImage("mcr.microsoft.com/azure-storage/azurite")
        .WithName($"Azure.Blob.Storage.{Guid.NewGuid()}")
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