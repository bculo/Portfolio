using MediatR;
using Newtonsoft.Json;
using Notification.Application.Constants;
using Notification.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.EventHandlers.Crypto
{
    public static class CryptoPriceUpdatedHandler
    {
        public class Handler : INotificationHandler<Notification>
        {
            private readonly INotificationService _notification;

            public Handler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                var groupId = $"{GroupPrefixConstants.CRYPTO}-{notification.Symbol.ToLower()}";

                await _notification.NotifyGroup(groupId, notification);
            }
        }

        public class Notification : INotification
        {
            public long Id { get; set; }
            public string? Symbol { get; set; }
            public string? Name { get; set; }
            public decimal Price { get; set; }
            public string? Currency { get; set; }
        }
    }
}
