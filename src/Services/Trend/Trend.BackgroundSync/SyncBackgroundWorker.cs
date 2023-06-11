using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
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
                    var lastSync = await GetLastSync(scope);

                    if (CanExecuteSync(scope, lastSync))
                    {
                        await StartSyncProcess(scope);
                    }
                }

                await Task.Delay(_options.SleepTimeMiliseconds);
            }
        }

        private async Task<SyncStatus> GetLastSync(IServiceScope scope)
        {
            try
            {
                var syncRepo = scope.ServiceProvider.GetRequiredService<ISyncStatusRepository>();
                return await syncRepo.GetLastValidSync();
            }
            catch(Exception e)
            {
                return default;
            }
        }

        private async Task StartSyncProcess(IServiceScope scope)
        {
            try
            {
                var endpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
                var endpoint = await endpointProvider.GetSendEndpoint(new Uri("queue:trend-execute-news-sync"));
                await endpoint.Send(new ExecuteNewsSync { });             
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        private bool CanExecuteSync(IServiceScope scope, SyncStatus status)
        {
            var time = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

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