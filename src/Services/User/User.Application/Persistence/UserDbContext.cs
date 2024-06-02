using Microsoft.EntityFrameworkCore;
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
