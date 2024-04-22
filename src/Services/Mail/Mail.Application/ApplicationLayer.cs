using System.Reflection;
using FluentValidation;
using Mail.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Time.Common;

namespace Mail.Application;

public static class ApplicationLayer
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUtcTimeProvider();
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        
        services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
    }
}