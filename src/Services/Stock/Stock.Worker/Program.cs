using Stock.Application;
using Stock.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        ApplicationLayer.AddServices(services, configuration);
        ApplicationLayer.AddClients(services, configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
