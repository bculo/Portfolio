using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Core;
using Crypto.Core.Entities;
using Time.Common.Contracts;

namespace Crypto.Infrastracture.Persistence
{
    public class CryptoDbContext : DbContext
    {
        private readonly IDateTime _time;

        public CryptoDbContext(DbContextOptions<CryptoDbContext> options, IDateTime time) : base(options)
        {
            _time = time;
        }

        public DbSet<Core.Entities.Crypto> Cryptos { get; set; }
        public DbSet<Core.Entities.CryptoPrice> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CryptoDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AttachDateTimeToEntities();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AttachDateTimeToEntities()
        {
            var currentTime = _time.DateTime;

            foreach (var item in ChangeTracker.Entries<Entity>())
            {
                item.Entity.CreatedOn = currentTime;
                item.Entity.ModifiedOn = currentTime;
            }
        }
    }
}
