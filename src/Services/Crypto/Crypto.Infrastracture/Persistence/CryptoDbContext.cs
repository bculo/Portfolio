using Crypto.Application.Interfaces.Persistence;
using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Time.Common.Contracts;

namespace Crypto.Infrastracture.Persistence
{
    public class CryptoDbContext : DbContext, ICryptoDbContext
    {
        private readonly IDateTime _time;

        public CryptoDbContext(DbContextOptions<CryptoDbContext> options, IDateTime time) : base(options)
        {
            _time = time;
        }

        public DbSet<Core.Entities.Crypto> Cryptos => Set<Core.Entities.Crypto>();
        public DbSet<Core.Entities.CryptoPrice> Prices => Set<Core.Entities.CryptoPrice>();
        public DbSet<Core.Entities.Visit> Visits => Set<Core.Entities.Visit>();

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
