using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Persistence;
using Crypto.Infrastracture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture
{
    public static class InfrastractureLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CryptoDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("CryptoDatabase"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }
    }
}
