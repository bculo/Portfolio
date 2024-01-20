using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notification.Hub.Interfaces;


namespace Notification.Hub
{
    [Authorize]
    public class PortfolioHub : Hub<ISignalRClient>
    {
        private readonly ILogger<PortfolioHub> _logger;

        public PortfolioHub(ILogger<PortfolioHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("OnConnectedAsync called");
            
            _logger.LogInformation(Context.ConnectionId);
            _logger.LogInformation(Context.UserIdentifier);
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("OnDisconnectedAsync called");

            return base.OnDisconnectedAsync(exception);
        }
    }
}
