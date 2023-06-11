using Crypto.Core.Entities;
using Crypto.Infrastracture.Consumers.State;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;

namespace Crypto.Infrastracture.Persistence
{
    public class CryptoDbContext : SagaDbContext
    {
        private readonly IDateTimeProvider _time;

        public CryptoDbContext(DbContextOptions<CryptoDbContext> options, 
            IDateTimeProvider time) 
            : base(options)
        {
            _time = time;
        }

        public DbSet<Core.Entities.Crypto> Cryptos => Set<Core.Entities.Crypto>();
        public DbSet<Core.Entities.CryptoPrice> Prices => Set<Core.Entities.CryptoPrice>();
        public DbSet<Core.Entities.Visit> Visits => Set<Core.Entities.Visit>();

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new AddCryptoItemStateMap(); }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CryptoDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AttachDateTimeToEntities();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AttachDateTimeToEntities()
        {
            var currentTime = _time.Now;

            foreach (var item in ChangeTracker.Entries<Entity>())
            {
                item.Entity.CreatedOn = currentTime;
                item.Entity.ModifiedOn = currentTime;
            }
        }
    }
}
