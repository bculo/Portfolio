﻿using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Clients;
using Crypto.Infrastracture.Persistence;
using Crypto.Infrastracture.Persistence.Interceptors;
using Crypto.Infrastracture.Persistence.Repositories;
using Crypto.Infrastracture.Services;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddHttpClient<ICryptoInfoService, CoinMarketCapClient>();
            services.AddHttpClient<ICryptoPriceService, CryptoCompareClient>();
        }
    }
}
