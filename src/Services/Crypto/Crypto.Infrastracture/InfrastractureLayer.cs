using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Clients;
using Crypto.Infrastracture.Constants;
using Crypto.Infrastracture.Persistence;
using Crypto.Infrastracture.Persistence.Interceptors;
using Crypto.Infrastracture.Persistence.Repositories;
using Crypto.Infrastracture.Services;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Crypto.Infrastracture
{
    public static class InfrastractureLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CryptoInfoApiOptions>(configuration.GetSection("CryptoInfoApiOptions"));
            services.Configure<CryptoPriceApiOptions>(configuration.GetSection("CryptoPriceApiOptions"));

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
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ICryptoInfoService, CoinMarketCapClient>();

            services.AddSingleton<IIdentiferHasher>(i => 
            {
                var hasher = new Hashids(configuration.GetValue<string>("IdentifierHasher:Salt"),
                    configuration.GetValue<int>("IdentifierHasher:HashLength"));
                return new IdentifierHasher(hasher);
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisOptions:ConnectionString"];
                options.InstanceName = configuration["RedisOptions:InstanceName"];
            });

            ConfigureCoinMarketCapClient(services, configuration);
            services.AddHttpClient<ICryptoPriceService, CryptoCompareClient>();
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
                return policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), retryNumber));
            });     
        }
    }
}
