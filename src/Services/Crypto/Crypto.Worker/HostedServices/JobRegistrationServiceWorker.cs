using Crypto.Worker.Interfaces;
using Hangfire;

namespace Crypto.Worker.HostedServices
{
    public class JobRegistrationServiceWorker(ILogger<JobRegistrationServiceWorker> logger, IServiceProvider provider)
        : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StartAsync method called in Background service {0}", nameof(JobRegistrationServiceWorker));
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StopAsync method called in Background service {0}", nameof(JobRegistrationServiceWorker));
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

                using var scope = provider.CreateScope();
                var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                jobManager.AddOrUpdate<IPriceUpdateJobService>(
                    "crypto-price-update-v2",
                    x => x.ExecuteUpdate(),
                    "*/3 * * * *");
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
            
            return Task.CompletedTask;
        }
    }
}
