using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Trend.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Trend;

public record SendSyncExecutedNotification(DateTime Time) : IRequest;

public class SendSyncExecutedNotificationHandler : IRequestHandler<SendSyncExecutedNotification>
{
    private readonly IDateTimeProvider _timeProvider;
    private readonly INotificationService _notification;
    private readonly ILogger<SendSyncExecutedNotificationHandler> _logger;

    public SendSyncExecutedNotificationHandler(INotificationService notification,
        IDateTimeProvider timeProvider,
        ILogger<SendSyncExecutedNotificationHandler> logger)
    {
        _notification = notification;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task Handle(SendSyncExecutedNotification request, CancellationToken cancellationToken)
    {
        if ((_timeProvider.Utc - request.Time).Minutes > 5)
        {
            _logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await _notification.NotifyAll(TrendNotifications.SyncExecuted);
    }
}