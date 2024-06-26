using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User.Application;
using User.Application.Interfaces;
using User.Functions.Middlewares;
using User.Functions.Options;
using User.Functions.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(opt =>
    {
        opt.UseMiddleware<ExceptionMiddleware>();
        opt.UseWhen<AuthorizationMiddleware>(context =>
        {
           return context.FunctionDefinition.InputBindings.Values
                     .First(a => a.Type.EndsWith("Trigger")).Type == "httpTrigger";
        });
    })
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        ApplicationLayer.AddServices(services, context.Configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>(_ => new CurrentUserService(Enumerable.Empty<Claim>()));
        services.AddScoped<ITokenService, JwtTokenService>();
        services.Configure<JwtValidationOptions>(context.Configuration.GetSection("JwtValidationOptions"));
    })
    .ConfigureOpenApi()
    .Build();

await host.RunAsync();

//For testing purpose
public partial class Program { }