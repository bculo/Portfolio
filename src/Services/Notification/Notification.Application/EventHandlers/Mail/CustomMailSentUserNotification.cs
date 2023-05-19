using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;

namespace Notification.Application.EventHandlers.Mail;

public static class CustomMailSentUserNotification
{
    public class Notification : INotification
    {
        public string MailId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : INotificationHandler<Notification>
    {
        private readonly INotificationService _notification;
        private readonly ILogger<Handler> _logger;

        public Handler(INotificationService notification,
            ILogger<Handler> logger)
        {
            _logger = logger;
            _notification = notification;
        }

        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}