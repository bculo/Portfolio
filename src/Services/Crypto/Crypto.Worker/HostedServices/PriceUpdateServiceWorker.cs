using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Options;
using Crypto.Worker.Interfaces;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Crypto.BackgroundUpdate.HostedServices
{
    public class PriceUpdateServiceWorker : Microsoft.Extensions.Hosting.BackgroundService
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
            _logger.LogInformation("StartAsync method called in Background service {0}", nameof(PriceUpdateServiceWorker));
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync method called in Background service {0}", nameof(PriceUpdateServiceWorker));
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

                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var endpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
                        var endpoint = await endpointProvider.GetSendEndpoint(new Uri($"queue:crypto-update-crypto-items-price"));
 
                        if(endpoint is null)
                        {
                            throw new CryptoCoreNotFoundException($"Message broker endpoint crypto-update-crypto-items-price not found");
                        }

                        await endpoint.Send(new UpdateCryptoItemsPrice { }, stoppingToken);
                        await unitOfWork.Commit(); //Outbox pattern commit

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