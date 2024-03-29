﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Behaviours;
using System.Reflection;
using Sqids;

namespace Stock.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(opt =>
            {
                opt.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddSingleton(new SqidsEncoder<int>(new()
            {
                MinLength = 20,
                Alphabet = configuration["Encoder:Alphabet"] 
                           ?? throw new ArgumentException("Encoder:Alphabet property is null")
            }));
            
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        }
   }
}
