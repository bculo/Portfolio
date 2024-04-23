using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Mail.Application.Interfaces.Mail;
using Mail.Application.Interfaces.Repository;
using Mail.Application.Options;
using Mail.Infrastructure.Mail;
using Mail.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mail.Infrastructure;

public static class InfrastructureLayer
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, MockMailService>();
        services.Configure<MailOptions>(configuration.GetSection(nameof(MailOptions)));
        
        services.Configure<AwsOptions>(configuration.GetSection(nameof(AwsOptions)));
        
        var credentials = new BasicAWSCredentials(
            configuration["AwsOptions:AccessKeyId"], 
            configuration["AwsOptions:AccessKeySecret"]);
            
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = configuration["AwsOptions:ServiceUrl"],
            AuthenticationRegion = configuration["AwsOptions:Region"]
        };
        
        services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(credentials, config));
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        
        services.AddScoped<IMailTemplateRepository, MailTemplateRepository>();
        services.AddScoped<IMailRepository, MailRepository>();
    }
}