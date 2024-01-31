using Stock.Worker.Configurations;
using Stock.Worker.HostedServices;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.ConfigureBackgroundService(configuration);
        services.AddHostedService<JobRegistrationServiceWorker>();
    })
    .Build();

host.Run();
