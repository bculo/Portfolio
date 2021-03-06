using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Crypto.BackgroundUpdate
{
    public class CryptoUpdateServiceWorker : BackgroundService
    {
        private readonly ILogger<CryptoUpdateServiceWorker> _logger;
        private readonly CryptoUpdateOptions _options;
        private readonly IServiceProvider _provider;

        public CryptoUpdateServiceWorker(ILogger<CryptoUpdateServiceWorker> logger,
            IOptions<CryptoUpdateOptions> options,
            IServiceProvider provider)
        {
            _logger = logger;
            _options = options.Value;
            _provider = provider;
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
                    catch(Exception e)
                    {
                        stopWatch.Stop();

                        _logger.LogError(e, e.Message);
                    }

                    executionTimeSpan = stopWatch.Elapsed;
                }

                Thread.Sleep(CalculateSleepTime(executionTimeSpan));
            }
        }

        private int CalculateSleepTime(TimeSpan executionTimeSpan)
        {
            return (_options.TimeSpanInSeconds * 1000) - (int)executionTimeSpan.TotalMilliseconds;
        }
    }
}