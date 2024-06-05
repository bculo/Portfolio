using Hangfire;
using Stock.Worker.Jobs;

namespace Stock.Worker.HostedServices
{
    public class JobRegistrationServiceWorker(ILogger<JobRegistrationServiceWorker> logger, IServiceProvider provider)
        : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "StartAsync method called in Background service {WorkerName}", 
                nameof(JobRegistrationServiceWorker));
            
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "StopAsync method called in Background service {WorkerName}",
                nameof(JobRegistrationServiceWorker));
            return base.StopAsync(cancellationToken);
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = provider.CreateScope();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var joManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                joManager.AddOrUpdate<ICreateBatchJob>(
                    configuration["RecurringJobsOptions:CreateBatchJobIdentifier"],
                    x => x.InitializeUpdateProcedure(),
                    configuration["RecurringJobsOptions:CreateBatchJobCron"]);

                logger.LogInformation("A Hangfire job with identifier {Identifier} successfully registered", 
                    configuration["RecurringJobsOptions:CreateBatchJobIdentifier"]);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return Task.CompletedTask;
        }
    }
}
