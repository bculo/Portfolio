using Hangfire;
using Stock.Worker.Jobs;

namespace Stock.Worker.HostedServices
{
    /// <summary>
    /// Background service that initialize all hanfire jobs. Its only executed once
    /// </summary>
    public class JobRegistrationServiceWorker : BackgroundService
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

        /// <summary>
        /// Register hangfire jobs
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var jobmanager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                jobmanager.AddOrUpdate<IPriceUpdateJobService>(
                    configuration["RecurringJobsOptions:UpdateStockPriceIdentifer"],
                    x => x.InitializeUpdateProcedure(),
                    configuration["RecurringJobsOptions:UpdateStockPriceCron"]);

                _logger.LogInformation("A Hanggire job with identifer {0} successfully registered", 
                    configuration["RecurringJobsOptions:UpdateStockPriceIdentifer"]);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
