using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache.Abstract.Contracts;
using MassTransit.Middleware;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Trend.Application.Consumers;

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
        var count = await _syncService.GetSyncCount(default);

        await _cacheService.SetAsync(CacheKeys.SYNC_TOTAL_COUNT,
            count,
            TimeSpan.FromHours(12));
    }
}

public class SyncExecutedConsumerDefinition : ConsumerDefinition<SyncExecutedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<SyncExecutedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
    }
}

