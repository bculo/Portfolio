using Microsoft.AspNetCore.SignalR;
using Notification.Application.Interfaces;
using Notification.Hub.Interfaces;

namespace Notification.Hub.Services
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly ILogger<SignalRNotificationService> _logger;
        private readonly IHubContext<PortfolioHub, ISignalRClient> _hubContext;

        public SignalRNotificationService(ILogger<SignalRNotificationService> logger,
            IHubContext<PortfolioHub, ISignalRClient> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
    }
}
