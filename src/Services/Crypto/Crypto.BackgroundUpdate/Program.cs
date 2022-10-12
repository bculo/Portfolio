using Crypto.Application;
using Crypto.BackgroundUpdate;
using Crypto.Infrastracture;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;

        ApplicationLayer.AddServices(services, configuration);
        InfrastractureLayer.AddServices(services, configuration);
        ApplicationLayer.ConfigureMessageQueue(services, configuration, false);

        services.AddHostedService<CryptoUpdateServiceWorker>();
    });

var host = hostBuilder.Build();

await host.RunAsync();