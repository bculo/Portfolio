using MediatR;
using Notification.Application.Interfaces.Notifications;
using Notification.Application.Interfaces.Notifications.Models;
using Notification.Application.Shared.Constants;
using Notification.Application.Shared.Group;

namespace Notification.Application.Features.Stock;

public record SendPriceUpdatedNotification(string Symbol, float Price) : IRequest;

public class SendPriceUpdatedNotificationHandler : IRequestHandler<SendPriceUpdatedNotification>
{
    private readonly INotificationService _notification;

    public SendPriceUpdatedNotificationHandler(INotificationService notification)
    {
        _notification = notification;
    }

    public async Task Handle(SendPriceUpdatedNotification request, CancellationToken cancellationToken)
    {
        var groupName = GroupUtilities.FormatGroupName(request.Symbol, GroupType.STOCK);
        PushNotification notification = new(groupName, request);
        await _notification.NotifyGroup(notification);
    }
}