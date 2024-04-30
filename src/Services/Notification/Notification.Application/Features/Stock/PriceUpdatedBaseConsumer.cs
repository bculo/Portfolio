namespace Notification.Application.Features.Stock;

public abstract class PriceUpdatedBaseConsumer
{
    protected SendPriceUpdatedNotification CreateNotification(string symbol, decimal price)
    {
        return new SendPriceUpdatedNotification(symbol, RoundToTwoDecimalPlaces(price));
    }

    private float RoundToTwoDecimalPlaces(decimal price)
    {
        return (float)Math.Round(price, 2);
    }
}