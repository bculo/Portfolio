using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace Tests.Common.Services.Containers;

public class TimescaleDBFixture : ContainerFixture
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("timescale/timescaledb:latest-pg14")
        .WithName($"TimescaleDB.{Guid.NewGuid()}")
        .Build();

    protected override DockerContainer Container => _container;
    public override string GetConnectionString()
    {
        return _container.GetConnectionString();
    }
}