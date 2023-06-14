using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Abstract.Contracts;

namespace Tracker.Infrastructure.Persistence
{
    public class TrackerDbContext : DbContext
    {
        private readonly IDateTimeProvider _time;
        private readonly IConfiguration _config;

        public DbSet<Core.Entities.FavoriteAsset> Favorites => Set<Core.Entities.FavoriteAsset>();

        public TrackerDbContext(IConfiguration config, IDateTimeProvider time)
        {
            _time = time;
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString("TrackerDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackerDbContext).Assembly);
        }
    }
}
