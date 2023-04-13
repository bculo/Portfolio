using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Clients;
using Crypto.Infrastracture.Constants;
using Crypto.Infrastracture.Consumers.State;
using Crypto.Infrastracture.Persistence;
using Crypto.Infrastracture.Persistence.Interceptors;
using Crypto.Infrastracture.Persistence.Repositories;
using Crypto.Infrastracture.Services;
using HashidsNet;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Reflection;

namespace Crypto.Infrastracture
{
    public static class InfrastractureLayer
    {
        public static void AddCommonServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IIdentiferHasher>(i => 
            {
                var hasher = new Hashids(configuration.GetValue<string>("IdentifierHasher:Salt"),
                    configuration.GetValue<int>("IdentifierHasher:HashLength"));
                return new IdentifierHasher(hasher);
            });
        }

        public static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CryptoDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("CryptoDatabase"));
                opt.AddInterceptors(new[] { new CommandInterceptor() });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICryptoExplorerRepository, CryptoExplorerRepository>();
            services.AddScoped<ICryptoPriceRepository, CryptoPriceRepository>();
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<ICryptoInfoService, CoinMarketCapClient>();
            services.AddScoped<ICryptoPriceService, CryptoCompareClient>();
        }

        public static void AddCacheMemory(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICacheService, CacheService>();
            services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });
        }

        public static void ConfigureWebProjectMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<CryptoDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });
                
                x.AddDelayedMessageScheduler();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Crypto", false));

                x.AddSagaStateMachine<AddCryptoItemStateMachine, AddCryptoItemState, AddCryptoItemStateMachineDefinition>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<CryptoDbContext>();
                        r.UseSqlServer();
                    });

                x.AddConsumers(Assembly.GetExecutingAssembly());

                x.UsingRabbitMq((context, config) =>
                {
                    config.UseDelayedMessageScheduler();
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CryptoInfoApiOptions>(configuration.GetSection("CryptoInfoApiOptions"));
            services.Configure<CryptoPriceApiOptions>(configuration.GetSection("CryptoPriceApiOptions"));

            ConfigureCoinMarketCapClient(services, configuration);
            ConfigureCryptoCompareClient(services, configuration);
        }

        private static void ConfigureCoinMarketCapClient(IServiceCollection services, IConfiguration configuration)
        {
            string baseAddress = configuration["CryptoInfoApiOptions:BaseUrl"];
            int retryNumber = configuration.GetValue<int>("CryptoInfoApiOptions:RetryNumber");
            int timeout = configuration.GetValue<int>("CryptoInfoApiOptions:Timeout");
            string headerKey = configuration["CryptoInfoApiOptions:HeaderKey"];
            string headerValue = configuration["CryptoInfoApiOptions:ApiKey"];

            ArgumentNullException.ThrowIfNull(baseAddress);
            ArgumentNullException.ThrowIfNull(headerKey);
            ArgumentNullException.ThrowIfNull(headerValue);

            services.AddHttpClient(ApiClient.CryptoInfo, client =>
            {
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
            {
                return policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber));
            });     
        }

        private static void ConfigureCryptoCompareClient(IServiceCollection services, IConfiguration configuration)
        {
            string baseAddress = configuration["CryptoPriceApiOptions:BaseUrl"];
            int retryNumber = configuration.GetValue<int>("CryptoPriceApiOptions:RetryNumber");
            int timeout = configuration.GetValue<int>("CryptoPriceApiOptions:Timeout");
            string headerKey = configuration["CryptoPriceApiOptions:HeaderKey"];
            string headerValue = configuration["CryptoPriceApiOptions:ApiKey"];

            ArgumentNullException.ThrowIfNull(baseAddress);
            ArgumentNullException.ThrowIfNull(headerKey);
            ArgumentNullException.ThrowIfNull(headerValue);

            services.AddHttpClient(ApiClient.CryptoPrice, client =>
            {
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
            {
                return policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber));
            });
        }
    }
}
