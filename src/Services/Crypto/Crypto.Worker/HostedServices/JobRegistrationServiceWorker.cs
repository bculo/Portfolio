using Crypto.Worker.Interfaces;
using Hangfire;

namespace Crypto.Worker.HostedServices
{
    public class JobRegistrationServiceWorker : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<JobRegistrationServiceWorker> _logger;
        private readonly IServiceProvider _provider;

        public JobRegistrationServiceWorker(ILogger<JobRegistrationServiceWorker> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync method called in Background service {0}", nameof(JobRegistrationServiceWorker));
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync method called in Background service {0}", nameof(JobRegistrationServiceWorker));
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var jobmanager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                jobmanager.AddOrUpdate<IPriceUpdateJobService>(
                    "crypto-price-update",
                    x => x.ExecuteUpdate(),
                    "*/30 * * * * *");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
