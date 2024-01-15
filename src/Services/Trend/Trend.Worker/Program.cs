using Trend.Application;
using Trend.Worker;
using Trend.Worker.Extensions;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        services.ConfigureBackgroundService(configuration);
        services.AddHostedService<PingWorker>();
    });

ApplicationLayer.AddLogger(hostBuilder);

var host = hostBuilder.Build();
await host.RunAsync();
