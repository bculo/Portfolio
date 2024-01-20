using MediatR;
using Notification.Application.Interfaces;

namespace Notification.Application.Features.Trend;

public record SendSyncExecutedNotificationCommand : IRequest;

public class SendSyncExecutedNotificationHandler : IRequestHandler<SendSyncExecutedNotificationCommand>
{
    private readonly INotificationService _notification;

    public SendSyncExecutedNotificationHandler(INotificationService notification)
    {
        _notification = notification;
    }

    public async Task Handle(SendSyncExecutedNotificationCommand request, CancellationToken cancellationToken)
    {
        await _notification.NotifyAll("Sync executed");
    }
}