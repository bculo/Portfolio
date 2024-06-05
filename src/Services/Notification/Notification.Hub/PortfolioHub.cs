using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notification.Hub.Interfaces;


namespace Notification.Hub
{
    [Authorize]
    public class PortfolioHub(ILogger<PortfolioHub> logger) : Hub<ISignalRClient>
    {
        public override Task OnConnectedAsync()
        {
            logger.LogTrace("Connection id {ConnectionId} connected", Context.ConnectionId);
            
            return base.OnConnectedAsync();
        }
        
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            logger.LogTrace("Connection id {ConnectionId} disconnected", Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
