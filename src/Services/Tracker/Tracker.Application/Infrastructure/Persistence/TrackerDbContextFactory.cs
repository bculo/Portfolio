using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Time.Common;

namespace Tracker.Application.Infrastructure.Persistence;

public class TrackerDbContextFactory : IDesignTimeDbContextFactory<TrackerDbContext>
{
    public TrackerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TrackerDbContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=Tracker;User Id=postgres;Password=florijan;");
        return new TrackerDbContext(optionsBuilder.Options, new LocalDateTimeService());
    }
}
