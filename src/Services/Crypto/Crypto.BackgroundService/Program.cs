using Crypto.BackgroundService.HostedServices;
using Crypto.BackgroundService.Interfaces;
using Crypto.BackgroundUpdate.Configurations;
using Crypto.BackgroundUpdate.HostedServices;
using Hangfire;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        services.ConfigureBackgroundService(configuration);;
        services.AddHostedService<JobRegistrationServiceWorker>();
    });

var host = hostBuilder.Build();

await host.RunAsync();