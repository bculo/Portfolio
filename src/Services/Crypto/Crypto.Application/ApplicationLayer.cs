using Crypto.Application.Behaviours;
using Crypto.Application.Options;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common;
using Time.Common.Contracts;

namespace Crypto.Application
{
    public class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));

            services.AddScoped<IDateTime, LocalDateTimeService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddMediatR(typeof(ApplicationLayer).Assembly);
            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
