using System.Reflection;
using Crypto.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Time.Common;

namespace Crypto.Application
{
    public class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddUtcTimeProvider();
            
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
            
            services.AddMediatR(opt =>
            {
                opt.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            
            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
        }
    }
}
