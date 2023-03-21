using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Crypto.BackgroundUpdate.HostedServices
{
    public class PriceUpdateServiceWorker : BackgroundService
    {
        private readonly ILogger<PriceUpdateServiceWorker> _logger;
        private readonly CryptoUpdateOptions _options;
        private readonly IServiceProvider _provider;

        public PriceUpdateServiceWorker(ILogger<PriceUpdateServiceWorker> logger,
            IOptions<CryptoUpdateOptions> options,
            IServiceProvider provider)
        {
            _logger = logger;
            _options = options.Value;
            _provider = provider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StartAsync method called in Background service {0}", nameof(PriceUpdateServiceWorker));

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StopAsync method called in Background service {0}", nameof(PriceUpdateServiceWorker));

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TimeSpan executionTimeSpan = default;

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    var stopWatch = new Stopwatch();

                    try
                    {
                        stopWatch.Start();

                        _logger.LogTrace("Scope created inside SyncBackgroundService");

                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Send(new UpdatePriceAllCommand { });

                        stopWatch.Stop();
                    }
                    catch (Exception e)
                    {
                        stopWatch.Stop();

                        _logger.LogError(e, e.Message);
                    }

                    executionTimeSpan = stopWatch.Elapsed;
                }

                await Task.Delay(CalculateSleepTime(executionTimeSpan), stoppingToken);
            }
        }

        private int CalculateSleepTime(TimeSpan executionTimeSpan)
        {
            var calculted = _options.TimeSpanInSeconds * 1000 - (int)executionTimeSpan.TotalMilliseconds;

            if (calculted < 0)
                return 0;

            return calculted;
        }
    }
}