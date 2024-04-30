using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Features.Stock.Types;

public static class StockNotifications
{
    public static PushNotification StatusChanged(string symbol, bool isActive) 
        => new("Stock.StatusChanged", new StockStatusChangedNotificationBody(symbol, isActive));
}

public record StockStatusChangedNotificationBody(string Symbol, bool IsActive);
