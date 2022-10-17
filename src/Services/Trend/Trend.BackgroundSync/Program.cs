using Trend.Application;
using Trend.BackgroundSync;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        ApplicationLayer.AddServices(configuration, services);

        services.AddHostedService<SyncBackgroundWorker>();
    });

ApplicationLayer.AddLogger(hostBuilder);

var host = hostBuilder.Build();

await host.RunAsync();
