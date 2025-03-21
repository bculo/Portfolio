﻿using Cache.Redis.Common;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Options;
using Crypto.Application.Interfaces.Information;
using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Infrastructure.Information;
using Crypto.Infrastructure.Persistence;
using Crypto.Infrastructure.Persistence.Repositories;
using Crypto.Infrastructure.Persistence.Repositories.Read;
using Crypto.Infrastructure.Price;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Crypto.Infrastructure
{
    public static class InfrastructureLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            AddPersistenceStorage(services, configuration);
            AddClients(services, configuration);
            AddHangfire(services, configuration);
            AddCache(services, configuration);

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateAsyncScope();
            using var db = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
            
            db.Database.Migrate();
        }

        private static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CryptoDbContext>();
            services.AddScoped<IDateSourceProvider, DataSourceProvider>();
            services.AddScoped<IConnectionProvider, DbContextConnectionProvider>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            
            services.AddScoped(typeof(IBaseReadRepository<>), typeof(BaseReadRepository<>));

            services.AddScoped<ICryptoPriceRepository, CryptoPriceRepository>();
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CryptoInfoApiOptions>(configuration.GetSection("CryptoInfoApiOptions"));
            services.Configure<CryptoPriceApiOptions>(configuration.GetSection("CryptoPriceApiOptions"));

            ConfigureCoinMarketCapClient(services, configuration);
            ConfigureCryptoCompareClient(services, configuration);
        }

        private static void ConfigureCoinMarketCapClient(IServiceCollection services, IConfiguration configuration)
        {
            var baseAddress = configuration["CryptoInfoApiOptions:BaseUrl"];
            var retryNumber = configuration.GetValue<int>("CryptoInfoApiOptions:RetryNumber");
            var timeout = configuration.GetValue<int>("CryptoInfoApiOptions:Timeout");
            var headerKey = configuration["CryptoInfoApiOptions:HeaderKey"];
            var headerValue = configuration["CryptoInfoApiOptions:ApiKey"];

            ArgumentNullException.ThrowIfNull(baseAddress);
            ArgumentNullException.ThrowIfNull(headerKey);
            ArgumentNullException.ThrowIfNull(headerValue);

            services.AddScoped<ICryptoInfoService, CoinMarketCapClient>();
            
            services.AddHttpClient(ApiClient.CryptoInfo, client =>
            {
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(
                Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber)));     
        }

        private static void ConfigureCryptoCompareClient(IServiceCollection services, IConfiguration configuration)
        {
            var baseAddress = configuration["CryptoPriceApiOptions:BaseUrl"];
            var retryNumber = configuration.GetValue<int>("CryptoPriceApiOptions:RetryNumber");
            var timeout = configuration.GetValue<int>("CryptoPriceApiOptions:Timeout");
            var headerKey = configuration["CryptoPriceApiOptions:HeaderKey"];
            var headerValue = configuration["CryptoPriceApiOptions:ApiKey"];

            ArgumentNullException.ThrowIfNull(baseAddress);
            ArgumentNullException.ThrowIfNull(headerKey);
            ArgumentNullException.ThrowIfNull(headerValue);
            
            //services.AddScoped<ICryptoPriceService, CryptoCompareClient>();
            services.AddScoped<ICryptoPriceService, MockPriceClient>();

            services.AddHttpClient(ApiClient.CryptoPrice, client =>
            {
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(
                Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber)));
        }


        private static void AddHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UsePostgreSqlStorage(opt =>
                {
                    opt.UseNpgsqlConnection(configuration.GetConnectionString("CryptoDatabase"));
                });
            });
        }

        private static void AddCache(IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var redisInstanceName = configuration["RedisOptions:InstanceName"];
            
            services.AddRedisFusionCacheService(redisConnectionString!, redisInstanceName!);
        } 
    }
}
