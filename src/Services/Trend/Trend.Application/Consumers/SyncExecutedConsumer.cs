using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache.Abstract.Contracts;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Trend.Application.Consumers
{
    public class SyncExecutedConsumer : IConsumer<SyncExecuted>
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<SyncExecutedConsumer> _logger;
        private readonly IFusionCache _cacheService;

        public SyncExecutedConsumer(ILogger<SyncExecutedConsumer> logger, 
            ISyncService syncService, 
            IFusionCache cacheService)
        {
            _logger = logger;
            _syncService = syncService;
            _cacheService = cacheService;
        }

        public async Task Consume(ConsumeContext<SyncExecuted> context)
        {
            await _cacheService.SetAsync(CacheKeys.SYNC_TOTAL_COUNT,
                () => _syncService.GetSyncCount(default),
                TimeSpan.FromHours(12),
                default);
        }
    }
}
