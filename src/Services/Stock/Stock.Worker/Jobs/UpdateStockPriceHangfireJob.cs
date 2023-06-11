using MassTransit.Mediator;
using Stock.Application.Features;

namespace Stock.Worker.Jobs
{
    public interface IPriceUpdateJobService
    {
        Task InitializeUpdateProcedure();
    }

    public class UpdateStockPriceHangfireJob : IPriceUpdateJobService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateStockPriceHangfireJob> _logger;

        public UpdateStockPriceHangfireJob(ILogger<UpdateStockPriceHangfireJob> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task InitializeUpdateProcedure()
        {
            _logger.LogTrace("Publishing UpdateAll.Command");
            await _mediator.Publish(new UpdateAll.Command { });
        }
    }
}
