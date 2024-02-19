using System.Reflection;
using Crypto.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Time.Abstract.Contracts;
using Time.Common;

namespace Crypto.Application
{
    public class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddUtcTimeProvider();
            
            services.AddMediatR(opt =>
            {
                opt.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                opt.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
            });
            
            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        }
    }
}
