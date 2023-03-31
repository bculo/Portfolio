using Microsoft.AspNetCore.SignalR;
using Notification.Application.Interfaces;
using Notification.Hub.Interfaces;

namespace Notification.Hub.Services
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<PortfolioHub, ISignalRClient> _hubContext;

        public SignalRNotificationService(IHubContext<PortfolioHub, ISignalRClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAll(string message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await _hubContext.Clients.All.Message(message);
        }

        public async Task NotifyAll(object message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await _hubContext.Clients.All.Message(message);
        }

        public async Task NotifyGroup(string groupId, string message)
        {
            ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await _hubContext.Clients.Group(groupId).Message(message);
        }

        public async Task NotifyGroup(string groupId, object message)
        {
            ArgumentNullException.ThrowIfNull(groupId, nameof(groupId));
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await _hubContext.Clients.Group(groupId).Message(message);
        }

        public async Task NotifyUser(string userId, string message)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await _hubContext.Clients.User(userId).Message(message);
        }

        public async Task NotifyUser(string userId, object message)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await _hubContext.Clients.User(userId).Message(message);
        }
    }
}
