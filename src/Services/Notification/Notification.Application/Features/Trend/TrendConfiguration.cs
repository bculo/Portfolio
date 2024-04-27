using MassTransit;
using MassTransit.Configuration;

namespace Notification.Application.Features.Trend;

public static class TrendConfiguration
{
    public static void ConfigureTrendConsumers(this IBusRegistrationConfigurator configurator) 
    {
        configurator.AddConsumer<SyncExecutedConsumer>();
        configurator.AddConsumer<ArticleActivatedConsumer>();
        configurator.AddConsumer<ArticleDeactivatedConsumer>();
        configurator.AddConsumer<SearchWordActivatedConsumer>();
        configurator.AddConsumer<SearchWordDeactivatedConsumer>();
    }
}