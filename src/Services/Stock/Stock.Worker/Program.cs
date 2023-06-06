using Stock.Worker.Configurations;
using Stock.Worker.HostedServices;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        services.ConfigureBackgroundService(configuration);
        services.AddHostedService<JobRegistrationServiceWorker>();
    })
    .Build();

host.Run();
