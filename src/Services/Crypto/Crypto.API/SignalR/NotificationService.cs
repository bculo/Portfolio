using Crypto.Application.Interfaces.Services;
using Events.Common.Crypto;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Crypto.API.SignalR
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<CryptoHub, ICryptoHubClient> _hub;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IHubContext<CryptoHub, ICryptoHubClient> hub, ILogger<NotificationService> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task NotifyAboutPriceUpdate(CryptoPriceUpdated updated)
        {
            _logger.LogTrace("NotifyAboutPriceUpdate for Symbol {0} called", updated.Symbol);

            await _hub.Clients.Group(updated.Symbol).ReceiveCryptoPriceUpdateNotificaiton(JsonConvert.SerializeObject(updated));
        }
    }
}
