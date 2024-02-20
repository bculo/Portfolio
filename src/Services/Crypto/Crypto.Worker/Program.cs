using Crypto.Worker.Configurations;
using Crypto.Worker.HostedServices;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        services.ConfigureBackgroundService(configuration);;
        services.AddHostedService<JobRegistrationServiceWorker>();
    });

var host = hostBuilder.Build();

await host.RunAsync();