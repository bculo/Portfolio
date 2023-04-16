using Trend.Application;
using Trend.BackgroundSync;
using Trend.BackgroundSync.Extensions;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        services.ConfigureBackgroundService(configuration);
        services.AddHostedService<SyncBackgroundWorker>();
    });

ApplicationLayer.AddLogger(hostBuilder);

var host = hostBuilder.Build();
await host.RunAsync();
