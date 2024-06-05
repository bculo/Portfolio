using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Stock.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Stock;

public record StockStatusChangedNotification(string Symbol, DateTime Time, bool IsActive) : IRequest;

public class StockStatusChangedNotificationHandler(
    INotificationService notification,
    IDateTimeProvider timeProvider,
    ILogger<StockStatusChangedNotificationHandler> logger)
    : IRequestHandler<StockStatusChangedNotification>
{
    public async Task Handle(StockStatusChangedNotification request, CancellationToken cancellationToken)
    {
        if ((timeProvider.Utc - request.Time).Minutes > 5)
        {
            logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await notification.NotifyAll(StockNotifications.StatusChanged(request.Symbol, request.IsActive));
    }
}