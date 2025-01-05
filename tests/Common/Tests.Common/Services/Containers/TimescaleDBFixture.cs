using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace Tests.Common.Services.Containers;

public class TimescaleDBFixture : ContainerFixture
{
    private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .SetMinimumLevel(LogLevel.Debug)
            .AddDebug()
            .AddConsole();
    });

    private readonly PostgreSqlBuilder _builder = new PostgreSqlBuilder()
        .WithImage("timescale/timescaledb:latest-pg14")
        .WithName($"TimescaleDB.{Guid.NewGuid()}")
        .WithCleanUp(true);

    private PostgreSqlContainer _container = default!;

    public TimescaleDBFixture()
    {
        _container = _builder.WithLogger(_loggerFactory.CreateLogger(nameof(TimescaleDBFixture)))
            .Build();
        
    }
    
    protected override DockerContainer Container => _container;
    public override string GetConnectionString()
    {
        return _container.GetConnectionString();
    }
}