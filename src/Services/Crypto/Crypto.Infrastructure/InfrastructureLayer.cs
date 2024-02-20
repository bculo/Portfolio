using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Core.Interfaces;
using Crypto.Infrastructure.Clients;
using Crypto.Infrastructure.Constants;
using Crypto.Infrastructure.Persistence;
using Crypto.Infrastructure.Persistence.Interceptors;
using Crypto.Infrastructure.Persistence.Repositories;
using Crypto.Infrastructure.Services;
using HashidsNet;
using MassTransit;
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
        }

        private static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CryptoDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("CryptoDatabase"));
            });
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<ICryptoInfoService, CoinMarketCapClient>();
            services.AddScoped<ICryptoPriceService, CryptoCompareClient>();
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

            services.AddHttpClient(ApiClient.CryptoPrice, client =>
            {
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(
                Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber)));
        }
    }
}
