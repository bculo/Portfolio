using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Core.ReadModels;
using Crypto.Infrastructure.Consumers.State;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;

namespace Crypto.Infrastructure.Persistence
{
    public class CryptoDbContext(
        DbContextOptions<CryptoDbContext> options,
        IConnectionProvider connectionProvider,
        IDateTimeProvider time)
        : SagaDbContext(options)
    {
        public virtual DbSet<Core.Entities.Crypto> Cryptos => Set<Core.Entities.Crypto>();
        public virtual DbSet<CryptoPrice> Prices => Set<CryptoPrice>();
        public virtual DbSet<CryptoLastPriceReadModel> CryptoLastPrice => Set<CryptoLastPriceReadModel>();

        #region FUNCTIONS

        public IQueryable<CryptoTimeFrameReadModel> CryptoTimeFrame(int notOlderMin, int timeBucketMin)
            => FromExpression(() => CryptoTimeFrame(notOlderMin, timeBucketMin));

        #endregion
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CryptoDbContext).Assembly);

            modelBuilder.Entity<CryptoTimeFrameReadModel>()
                .ToTable(nameof(CryptoTimeFrameReadModel), opt => opt.ExcludeFromMigrations());
            
            modelBuilder.HasDbFunction(typeof(CryptoDbContext).GetMethod(
                    nameof(CryptoTimeFrame),
                    new[] { typeof(int), typeof(int) }),
                b =>
                {
                    b.IsBuiltIn(false);
                    b.HasName("get_data_by_timeframe");
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionProvider.GetConnectionString());
            optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new AddCryptoItemStateMap(); }
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AttachDateTimeToEntities();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AttachDateTimeToEntities()
        {
            var currentTime = time.Now;

            foreach (var item in ChangeTracker.Entries<Entity>())
            {
                item.Entity.CreatedOn = currentTime;
                item.Entity.ModifiedOn = currentTime;
            }
        }
    }
}
