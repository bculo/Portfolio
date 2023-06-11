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
            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddMediatR(typeof(ApplicationLayer).Assembly);
            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        }
    }
}
