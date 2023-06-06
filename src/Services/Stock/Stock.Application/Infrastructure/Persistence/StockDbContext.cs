using Microsoft.EntityFrameworkCore;
using Stock.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Infrastructure.Persistence
{
    public class StockDbContext : DbContext
    {
        public DbSet<Core.Entities.Stock> Stocks { get; set; }
        public DbSet<StockPrice> Prices { get; set; }

        public StockDbContext(DbContextOptions<StockDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
        }
    }
}
