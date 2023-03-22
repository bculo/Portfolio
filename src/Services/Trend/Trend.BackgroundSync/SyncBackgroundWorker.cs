using Microsoft.Extensions.Options;
using Time.Common.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Options;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;

namespace Trend.BackgroundSync
{
    public class SyncBackgroundWorker : BackgroundService
    {
        private readonly ILogger<SyncBackgroundWorker> _logger;
        private readonly IServiceProvider _provider;
        private readonly SyncBackgroundServiceOptions _options;

        public SyncBackgroundWorker(ILogger<SyncBackgroundWorker> logger,
            IServiceProvider provider,
            IOptions<SyncBackgroundServiceOptions> options)
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

                    var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
                    var syncRepo = scope.ServiceProvider.GetRequiredService<ISyncStatusRepository>();

                    var lastSync = await syncRepo.GetLastValidSync();

                    if (CanExecuteSync(time, lastSync))
                    {
                        _logger.LogTrace("CanExecuteSync returned TRUE");
                        await StartSyncProcess(scope, time);
                    }
                }

                 await Task.Delay(_options.SleepTimeMiliseconds);
            }
        }

        private async Task StartSyncProcess(IServiceScope scope, IDateTimeProvider time)
        {
            try
            {
                var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();

                _logger.LogTrace("Sync started {0}", time.Now);

                var syncResult = await syncService.ExecuteGoogleSync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        private bool CanExecuteSync(IDateTimeProvider time, SyncStatus status)
        {
            if (status is null)
            {
                return true;
            }

            var timeSpanFromLastSync = time.Now - status.Finished!.Value;

            if (timeSpanFromLastSync.TotalHours > _options.TimeSpanBetweenSyncsHours)
            {
                _logger.LogInformation("Timespan between current time and last sync is greater then {0}", _options.TimeSpanBetweenSyncsHours);
                return true;
            }

            return false;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SyncBackgroundService stopped working!!!");

            return base.StopAsync(cancellationToken);
        }
    }
}