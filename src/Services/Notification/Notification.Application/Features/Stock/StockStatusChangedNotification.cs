using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Stock.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Stock;

public record StockStatusChangedNotification(string Symbol, DateTime Time, bool IsActive) : IRequest;

public class StockStatusChangedNotificationHandler : IRequestHandler<StockStatusChangedNotification>
{
    private readonly IDateTimeProvider _timeProvider;
    private readonly INotificationService _notification;
    private readonly ILogger<StockStatusChangedNotificationHandler> _logger;

    public StockStatusChangedNotificationHandler(INotificationService notification,
        IDateTimeProvider timeProvider,
        ILogger<StockStatusChangedNotificationHandler> logger)
    {
        _notification = notification;
        _timeProvider = timeProvider;
        _logger = logger;
    }
    
    public async Task Handle(StockStatusChangedNotification request, CancellationToken cancellationToken)
    {
        if ((_timeProvider.Utc - request.Time).Minutes > 5)
        {
            _logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await _notification.NotifyAll(StockNotifications.StatusChanged(request.Symbol, request.IsActive));
    }
}