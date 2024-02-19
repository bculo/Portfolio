﻿using Crypto.Core.Entities;
using Crypto.Infrastructure.Consumers.State;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;

namespace Crypto.Infrastructure.Persistence
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

        public virtual DbSet<Core.Entities.Crypto> Cryptos => Set<Core.Entities.Crypto>();
        public virtual DbSet<CryptoPrice> Prices => Set<CryptoPrice>();
        public virtual DbSet<CryptoLastPrice> CryptoLastPricesView => Set<CryptoLastPrice>();
        public virtual DbSet<Visit> Visits => Set<Visit>();

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