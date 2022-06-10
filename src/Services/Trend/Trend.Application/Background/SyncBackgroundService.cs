using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Options;

namespace Trend.Application.Background
{
    public class SyncBackgroundService : BackgroundService
    {
        private readonly ILogger<SyncBackgroundService> _logger;
        private readonly IServiceProvider _provider;
        private readonly SyncBackgroundServiceOptions _options;

        public SyncBackgroundService(ILogger<SyncBackgroundService> logger, IServiceProvider provider, IOptions<SyncBackgroundServiceOptions> options)
        {
            _logger = logger;
            _provider = provider;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    _logger.LogTrace("Scope created inside SyncBackgroundService");

                    try
                    {
                        var syncService = scope.ServiceProvider.GetRequiredService<IGoogleSyncService>();

                        var syncResult = await syncService.Sync(null);
                    }
                    catch(Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }

                    Thread.Sleep(_options.SleepTimeMiliseconds);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SyncBackgroundService stopped working!!!");

            return base.StopAsync(cancellationToken);
        }
    }
}
