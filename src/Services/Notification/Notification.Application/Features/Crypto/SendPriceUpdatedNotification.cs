using MediatR;
using Notification.Application.Interfaces.Notifications;
using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Features.Crypto;

public record SendPriceUpdatedNotification(string Symbol, decimal Price) : IRequest;

public class SendPriceUpdatedNotificationHandler(INotificationService notificationService)
    : IRequestHandler<SendPriceUpdatedNotification>
{

    public async Task Handle(SendPriceUpdatedNotification request, CancellationToken cancellationToken)
    {
        PushNotification notification = new(request.Symbol, request);
        await notificationService.NotifyGroup(notification);
    }
}