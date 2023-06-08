using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stock.Application.Interfaces;
using Stock.Core.Entities;
using Time.Common.Contracts;

namespace Stock.Application.Infrastructure.Persistence
{
    public class StockDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IStockUser _currentUser;
        private readonly IDateTimeProvider _timeProvider;

        public virtual DbSet<Core.Entities.Stock> Stocks { get; set; }
        public virtual DbSet<StockPrice> Prices { get; set; }

        public StockDbContext(IConfiguration configuration, 
            IDateTimeProvider timeprovider, 
            IStockUser currentUser)
        {
            _configuration = configuration;
            _timeProvider = timeprovider;
            _currentUser = currentUser;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("StockDatabase"));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>()
                       .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if(entry.State == EntityState.Added)
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
