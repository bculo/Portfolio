using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Trend.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Trend;

public record ArticleStatusChangedNotification(string ArticleId, bool IsActive, DateTime Time) : IRequest;

public class ArticleStatusChangedNotificationHandler(
    INotificationService notification,
    IDateTimeProvider timeProvider,
    ILogger<ArticleStatusChangedNotificationHandler> logger)
    : IRequestHandler<ArticleStatusChangedNotification>
{
    public async Task Handle(ArticleStatusChangedNotification request, CancellationToken cancellationToken)
    {
        if ((timeProvider.Utc - request.Time).Minutes > 5)
        {
            logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await notification.NotifyAll(TrendNotifications.ArticleStatusChanged);
    }
}
