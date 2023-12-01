using Amazon.Runtime.Internal.Util;
using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;

namespace Trend.Application.Consumers
{
    public class ExecuteNewsSyncConsumer : IConsumer<ExecuteNewsSync>
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<ExecuteNewsSyncConsumer> _logger;

        public ExecuteNewsSyncConsumer(ISyncService syncService, 
            ILogger<ExecuteNewsSyncConsumer> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ExecuteNewsSync> context)
        {
            try
            {
                await _syncService.ExecuteSync(default);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
