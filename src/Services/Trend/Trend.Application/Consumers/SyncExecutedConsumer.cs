using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Middleware;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace Trend.Application.Consumers;

public class SyncExecutedConsumer(
    ILogger<SyncExecutedConsumer> logger,
    ISyncService syncService,
    IFusionCache cacheService)
    : IConsumer<SyncExecuted>
{
    private readonly ILogger<SyncExecutedConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<SyncExecuted> context)
    {
        var count = await syncService.GetAllCount(default);

        await cacheService.SetAsync(CacheKeys.SyncTotalCount,
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

