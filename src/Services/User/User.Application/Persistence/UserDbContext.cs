using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Entities;

namespace User.Application.Persistence
{
    public class UserDbContext : DbContext
    {
        public virtual DbSet<PortfolioUser> Users => Set<PortfolioUser>();

        protected UserDbContext() { }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        }
    }
}
