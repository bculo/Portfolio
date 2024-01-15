using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache.Abstract.Contracts;
using Trend.Application.Interfaces;

namespace Trend.Application.Consumers
{
    public class SyncExecutedConsumer : IConsumer<SyncExecuted>
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<SyncExecutedConsumer> _logger;
        private readonly ICacheService _cacheService;

        public SyncExecutedConsumer(ILogger<SyncExecutedConsumer> logger, 
            ISyncService syncService, 
            ICacheService cacheService)
        {
            _logger = logger;
            _syncService = syncService;
            _cacheService = cacheService;
        }

        public async Task Consume(ConsumeContext<SyncExecuted> context)
        {
            var countNum = _syncService.GetSyncCount(default);
            await _cacheService.AddWithAbsoluteExp("sync:count", countNum, TimeSpan.FromDays(1));
        }
    }
}
