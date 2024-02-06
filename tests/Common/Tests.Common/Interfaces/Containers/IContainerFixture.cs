using Xunit;

namespace Tests.Common.Interfaces.Containers;

public interface IContainerFixture : IAsyncLifetime
{
    Task StartAsync();
    Task StopAsync();
    string GetConnectionString();
}