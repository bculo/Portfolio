namespace Tests.Common.Interfaces.Containers;

public interface IContainerFixture
{
    Task StartAsync();
    Task StopAsync();
    string GetConnectionString();
}