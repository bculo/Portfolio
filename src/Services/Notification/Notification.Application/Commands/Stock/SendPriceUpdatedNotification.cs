using MediatR;
using Notification.Application.Interfaces.Notifications;
using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Commands.Stock;

public record SendPriceUpdatedNotification(string Symbol, decimal Price) : IRequest;

public class SendPriceUpdatedNotificationHandler : IRequestHandler<SendPriceUpdatedNotification>
{
    private readonly INotificationService _notification;

    public SendPriceUpdatedNotificationHandler(INotificationService notification)
    {
        _notification = notification;
    }

    public async Task Handle(SendPriceUpdatedNotification request, CancellationToken cancellationToken)
    {
        PushNotification notification = new(request.Symbol, request);
        await _notification.NotifyGroup(request.Symbol, notification);
    }
}