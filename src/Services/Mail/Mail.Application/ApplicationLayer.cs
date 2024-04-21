using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using FluentValidation;
using Mail.Application.Behaviours;
using Mail.Application.Options;
using Mail.Application.Services.Implementations;
using Mail.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Time.Abstract.Contracts;
using Time.Common;

namespace Mail.Application;

public static class ApplicationLayer
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUtcTimeProvider();
        services.AddScoped<IEmailService, SendGridMailservice>();
        services.Configure<MailOptions>(configuration.GetSection(nameof(MailOptions)));
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        
        services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        
        AddPersitence(services, configuration);
    }

    private static void AddPersitence(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsOptions>(configuration.GetSection(nameof(AwsOptions)));
        
        var creds = new BasicAWSCredentials(configuration["AwsOptions:AccessKeyId"], configuration["AwsOptions:AccessKeySecret"]);
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = configuration["AwsOptions:ServiceUrl"],
            AuthenticationRegion = configuration["AwsOptions:Region"]
        };
        
        services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(creds, config));
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        
        services.AddScoped<IMailTemplateRepository, MailTemplateRepository>();
        services.AddScoped<IMailRepository, MailRepository>();
    }
}