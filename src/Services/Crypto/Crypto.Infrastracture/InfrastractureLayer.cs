using Crypto.Infrastracture.Persistence;
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
        }
    }
}
