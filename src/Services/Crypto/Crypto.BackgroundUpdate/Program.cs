using Crypto.Application;
using Crypto.BackgroundUpdate;
using Crypto.BackgroundUpdate.HostedServices;
using Crypto.Infrastracture;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;

        ApplicationLayer.AddServices(services, configuration);
        InfrastractureLayer.AddServices(services, configuration);
        InfrastractureLayer.ConfigureMessageQueue(services, configuration);

        services.AddHostedService<PriceUpdateServiceWorker>();
    });

var host = hostBuilder.Build();

await host.RunAsync();