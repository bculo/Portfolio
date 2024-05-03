using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Options;

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
            PeriodicTimer timer = new(new TimeSpan(0, 0, 30));
            var serviceIdentifier = Guid.NewGuid().ToString();
            while (!await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _provider.CreateScope();
                var timeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
                _logger.LogInformation("Server {Server} - ping at {Time}", serviceIdentifier, timeProvider.Utc);
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