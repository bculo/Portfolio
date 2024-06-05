using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Trend.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Trend;

public record SendSyncExecutedNotification(DateTime Time) : IRequest;

public class SendSyncExecutedNotificationHandler(
    INotificationService notification,
    IDateTimeProvider timeProvider,
    ILogger<SendSyncExecutedNotificationHandler> logger)
    : IRequestHandler<SendSyncExecutedNotification>
{
    public async Task Handle(SendSyncExecutedNotification request, CancellationToken cancellationToken)
    {
        if ((timeProvider.Utc - request.Time).Minutes > 5)
        {
            logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await notification.NotifyAll(TrendNotifications.SyncExecuted);
    }
}