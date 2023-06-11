using FluentValidation;
using Grpc.Net.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Reflection;
using Time.Abstract.Contracts;
using Time.Common;
using Tracker.Application.Behaviours;
using Tracker.Application.Constants;
using Tracker.Application.Infrastructure.Persistence;
using Tracker.Application.Infrastructure.Services;
using Tracker.Application.Interfaces;
using Tracker.Application.Options;

namespace Tracker.Application;

public static class ApplicationLayer
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EndpointgRPCOptions>(configuration.GetSection(nameof(EndpointgRPCOptions)));
        services.Configure<ApplicationInfoOptions>(configuration.GetSection(nameof(ApplicationInfoOptions)));
        
        services.AddScoped(services =>
        {
            var config = services.GetRequiredService<IOptionsSnapshot<EndpointgRPCOptions>>().Value;
            var channel = GrpcChannel.ForAddress(config.CryptoEndpoint);
            return new CryptogRPCAssetClient(new Crypto.gRPC.Protos.v1.Crypto.CryptoClient(channel));
        });
        
        services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
        services.AddScoped<IFinancialAssetClientFactory, FinancialAssetClientFactory>();
        
        services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
    }

    public static void AddCache(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITrackerCacheService, TrackerCacheService>();
        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["RedisOptions:ConnectionString"];
            options.InstanceName = configuration["RedisOptions:InstanceName"];
        });
    }
    
    public static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TrackerDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("TrackerDatabase"));
        });
    }

    public static void AddClients(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CryptoHttpAssetClient>();
        services.AddHttpClient(HttpClientNames.CRYPTO_CLIENT, client =>
            {
                client.BaseAddress = new Uri("http://localhost:5263/api/");
                client.Timeout = TimeSpan.FromSeconds(60);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
            {
                return policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 2));
            });
    }
}