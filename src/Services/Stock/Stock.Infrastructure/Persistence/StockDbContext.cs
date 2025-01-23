using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces.User;
using Stock.Core.Models.Base;
using Time.Abstract.Contracts;

namespace Stock.Infrastructure.Persistence
{
    public class StockDbContext(
        IConfiguration configuration,
        IDateTimeProvider timeProvider,
        IStockUser currentUser)
        : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("StockDatabase"));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>()
                       .Where(x => x.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in entries)
            {   
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = timeProvider.Now;
                    entry.Entity.CreatedBy = currentUser.Identifier.ToString();
                }

                entry.Entity.ModifiedAt = timeProvider.Now;
                entry.Entity.ModifiedBy = currentUser.Identifier.ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
