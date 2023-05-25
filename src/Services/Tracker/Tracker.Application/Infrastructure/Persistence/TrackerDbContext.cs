using Microsoft.EntityFrameworkCore;
using Time.Common.Contracts;

namespace Tracker.Application.Infrastructure.Persistence;

public class TrackerDbContext : DbContext
{
    private readonly IDateTimeProvider _time;
    
    public DbSet<Core.Entities.FavoriteAsset> Favorites => Set<Core.Entities.FavoriteAsset>();
    
    public TrackerDbContext(DbContextOptions<TrackerDbContext> options,
        IDateTimeProvider time)
        : base(options)
    {
        _time = time;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackerDbContext).Assembly);
    }
}