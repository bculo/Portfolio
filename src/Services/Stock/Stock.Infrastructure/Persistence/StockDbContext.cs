using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stock.Application.Interfaces.User;
using Stock.Core.Models;
using Stock.Core.Models.Base;
using Stock.Core.Models.Stock;
using Time.Abstract.Contracts;

namespace Stock.Infrastructure.Persistence
{
    public class StockDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IStockUser _currentUser;
        private readonly IDateTimeProvider _timeProvider;

        public virtual DbSet<StockEntity> Stocks => Set<StockEntity>();
        public virtual DbSet<StockPriceEntity> Prices => Set<StockPriceEntity>();
        
        public virtual DbSet<StockWithPriceTagReadModel> StockWithPriceTag => Set<StockWithPriceTagReadModel>();
        

        public StockDbContext(IConfiguration configuration,
            IDateTimeProvider timeProvider,
            IStockUser currentUser)
        {
            _configuration = configuration;
            _timeProvider = timeProvider;
            _currentUser = currentUser;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("StockDatabase"));
            optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
                    entry.Entity.CreatedAt = _timeProvider.Now;
                    entry.Entity.CreatedBy = _currentUser.Identifier.ToString();
                }

                entry.Entity.ModifiedAt = _timeProvider.Now;
                entry.Entity.ModifiedBy = _currentUser.Identifier.ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
