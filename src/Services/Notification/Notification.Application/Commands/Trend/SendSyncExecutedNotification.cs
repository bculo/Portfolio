using MediatR;
using Notification.Application.Common.Codes;
using Notification.Application.Interfaces.Notifications;

namespace Notification.Application.Commands.Trend;

public record SendSyncExecutedNotification : IRequest;

public class SendSyncExecutedNotificationHandler : IRequestHandler<SendSyncExecutedNotification>
{
    private readonly INotificationService _notification;

    public SendSyncExecutedNotificationHandler(INotificationService notification)
    {
        _notification = notification;
    }

    public async Task Handle(SendSyncExecutedNotification request, CancellationToken cancellationToken)
    {
        await _notification.NotifyAll(TrendNotifications.SyncExecuted);
    }
}