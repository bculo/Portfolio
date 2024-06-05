using Microsoft.AspNetCore.SignalR;
using Notification.Application.Interfaces.Notifications;
using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Hub.Services
{
    public class SignalRNotificationService(IHubContext<PortfolioHub> hubContext) : INotificationService
    {
        public async Task NotifyAll(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            
            await hubContext.Clients.All.SendAsync(notification.MethodName, notification.Content);
        }

        public async Task NotifyGroup(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            
            await hubContext.Clients.Group(notification.MethodName).SendAsync(notification.MethodName, notification.Content);
        }
        
        public async Task NotifyUser(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            
            await hubContext.Clients.User(notification.MethodName).SendAsync(notification.MethodName, notification.Content);
        }
    }
}
