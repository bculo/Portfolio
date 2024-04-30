using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task NotifyAll(PushNotification notification);
        Task NotifyGroup(PushNotification notification);
        Task NotifyUser(PushNotification notification);
    }
}
