using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Core;

namespace Crypto.Infrastracture.Persistence
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options)
        {

        }

        public DbSet<Core.Entities.Crypto> Cryptos { get; set; }
        public DbSet<Core.Entities.CryptoPrice> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CryptoDbContext).Assembly);
        }
    }
}
