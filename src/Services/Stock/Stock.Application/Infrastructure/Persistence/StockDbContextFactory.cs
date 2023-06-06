using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common;

namespace Stock.Application.Infrastructure.Persistence
{
    public class StockDbContextFactory : IDesignTimeDbContextFactory<StockDbContext>
    {
        public StockDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockDbContext>();
            optionsBuilder.UseNpgsql("HELLOWORLD");
            return new StockDbContext(optionsBuilder.Options);
        }
    }
}
