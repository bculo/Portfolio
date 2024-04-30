using Microsoft.AspNetCore.SignalR;
using Notification.Application.Interfaces;
using Notification.Application.Interfaces.Notifications;
using Notification.Application.Interfaces.Notifications.Models;
using Notification.Hub.Interfaces;

namespace Notification.Hub.Services
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<PortfolioHub> _hubContext;

        public SignalRNotificationService(IHubContext<PortfolioHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async Task NotifyAll(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            
            await _hubContext.Clients.All.SendAsync(notification.MethodName, notification.Content);
        }

        public async Task NotifyGroup(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            await _hubContext.Clients.Group(notification.MethodName).SendAsync(notification.MethodName, notification.Content);
        }
        
        public async Task NotifyUser(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            await _hubContext.Clients.User(notification.MethodName).SendAsync(notification.MethodName, notification.Content);
        }
    }
}
