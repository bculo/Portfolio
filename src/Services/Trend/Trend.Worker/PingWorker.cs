using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;

namespace Trend.Worker
{
    public class PingWorker : BackgroundService
    {
        private readonly ILogger<PingWorker> _logger;
        private readonly IServiceProvider _provider;
        private readonly SyncBackgroundServiceOptions _options;

        public PingWorker(ILogger<PingWorker> logger,
            IServiceProvider provider,
            IOptions<SyncBackgroundServiceOptions> options)
        {
            _logger = logger;
            _provider = provider;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceIdentifier = Guid.NewGuid().ToString();
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var timeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
                _logger.LogInformation($"SERVER {serviceIdentifier} PING {timeProvider.Now}");
                await Task.Delay(_options.SleepTimeMilliseconds, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SyncBackgroundService started working!");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SyncBackgroundService stopped working!");
            return base.StopAsync(cancellationToken);
        }
    }
}