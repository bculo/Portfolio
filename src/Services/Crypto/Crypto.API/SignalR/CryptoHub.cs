using Microsoft.AspNetCore.SignalR;

namespace Crypto.API.SignalR
{
    public class CryptoHub : Hub<ICryptoHubClient>
    {
        private readonly ILogger<CryptoHub> _logger;

        public CryptoHub(ILogger<CryptoHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
