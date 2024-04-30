using MassTransit;
using Notification.Application.Features.Crypto;
using Notification.Application.Features.Trend;

namespace Notification.Application.Features.Stock;

public static class StockConfiguration
{
    public static void ConfigureStockConsumers(this IBusRegistrationConfigurator configurator) 
    {
        configurator.AddConsumer<NewStockAddedConsumer>();
        configurator.AddConsumer<StockPriceUpdatedConsumer>();
        configurator.AddConsumer<StockDeactivatedConsumer>();
        configurator.AddConsumer<StockActivatedConsumer>();
    }
}