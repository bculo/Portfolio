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

        public async Task NotifyGroup(string groupId, PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            await _hubContext.Clients.Group(groupId).SendAsync(notification.MethodName, notification.Content);
        }
        
        public async Task NotifyUser(string userId, PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            await _hubContext.Clients.User(userId).SendAsync(notification.MethodName, notification.Content);
        }
    }
}
