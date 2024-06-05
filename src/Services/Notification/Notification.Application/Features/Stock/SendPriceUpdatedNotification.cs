using MediatR;
using Notification.Application.Interfaces.Notifications;
using Notification.Application.Interfaces.Notifications.Models;
using Notification.Application.Shared.Constants;
using Notification.Application.Shared.Group;

namespace Notification.Application.Features.Stock;

public record SendPriceUpdatedNotification(string Symbol, float Price) : IRequest;

public class SendPriceUpdatedNotificationHandler(INotificationService notification)
    : IRequestHandler<SendPriceUpdatedNotification>
{
    public async Task Handle(SendPriceUpdatedNotification request, CancellationToken cancellationToken)
    {
        var groupName = GroupUtilities.FormatGroupName(request.Symbol, GroupType.Stock);
        PushNotification notification1 = new(groupName, request);
        await notification.NotifyGroup(notification1);
    }
}