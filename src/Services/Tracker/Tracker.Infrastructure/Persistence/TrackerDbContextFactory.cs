using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common;
using Microsoft.Extensions.Configuration;

namespace Tracker.Infrastructure.Persistence
{
    public class TrackerDbContextFactory : IDesignTimeDbContextFactory<TrackerDbContext>
    {
        public TrackerDbContext CreateDbContext(string[] args)
        {
            //Debugger.Launch(); Uncomment for debug feature

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            return new TrackerDbContext(config, null);
        }
    }
}
