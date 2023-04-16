using Events.Common.Trend;
using MassTransit;
using Trend.Application.Interfaces;

namespace Trend.Application.Consumers
{
    public class ExecuteNewsSyncConsumer : IConsumer<ExecuteNewsSync>
    {
        private readonly ISyncService _syncService;

        public ExecuteNewsSyncConsumer(ISyncService syncService)
        {
            _syncService = syncService;
        }

        public async Task Consume(ConsumeContext<ExecuteNewsSync> context)
        {
            await _syncService.ExecuteGoogleSync();
        }
    }
}
