using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using User.Application;
using User.Functions.Middlewares;
using User.Functions.Options;
using User.Functions.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(opt =>
    {
        opt.UseMiddleware<ExceptionMiddleware>();
        opt.UseMiddleware<AuthorizationMiddleware>();
    })
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        ApplicationLayer.AddServices(services, context.Configuration);

        services.AddScoped<IUserService, CurrentUserService>(services =>
        {
            return new CurrentUserService(Enumerable.Empty<Claim>());
        });

        services.AddScoped<ITokenService, JwtTokenService>();
        services.Configure<JwtValidationOptions>(context.Configuration.GetSection("JwtValidationOptions"));
    })
    .Build();

host.Run();
