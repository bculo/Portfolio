using Microsoft.AspNetCore.SignalR;
using Notification.Hub.Interfaces;

namespace Notification.Hub
{
    public class PortfolioHub : Hub<ISignalRClient>
    {
        private readonly ILogger<PortfolioHub> _logger;

        public PortfolioHub(ILogger<PortfolioHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogTrace("OnConnectedAsync called");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogTrace("OnDisconnectedAsync called");

            return base.OnDisconnectedAsync(exception);
        }
    }
}
