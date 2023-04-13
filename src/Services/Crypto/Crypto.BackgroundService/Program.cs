using Crypto.BackgroundUpdate.Configurations;
using Crypto.BackgroundUpdate.HostedServices;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        services.ConfigureBackgroundService(configuration);
        services.AddHostedService<PriceUpdateServiceWorker>();
    });

var host = hostBuilder.Build();

await host.RunAsync();