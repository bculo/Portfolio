using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Trend.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Trend;

public record SearchWordStatusChangedNotification(string SearchWordId, DateTime Time) : IRequest;


public class SearchWordStatusChangedNotificationHandler(
    INotificationService notification,
    IDateTimeProvider timeProvider,
    ILogger<SearchWordStatusChangedNotificationHandler> logger)
    : IRequestHandler<SearchWordStatusChangedNotification>
{
    public async Task Handle(SearchWordStatusChangedNotification request, CancellationToken cancellationToken)
    {
        if ((timeProvider.Utc - request.Time).Minutes > 5)
        {
            logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await notification.NotifyAll(TrendNotifications.SearchWordStatusChanged);
    }
}