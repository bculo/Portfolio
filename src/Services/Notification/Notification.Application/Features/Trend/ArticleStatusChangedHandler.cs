using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Trend.Types;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Trend;

public record ArticleStatusChangedNotification(string ArticleId, bool IsActive, DateTime Time) : IRequest;

public class ArticleStatusChangedNotificationHandler : IRequestHandler<ArticleStatusChangedNotification>
{
    private readonly IDateTimeProvider _timeProvider;
    private readonly INotificationService _notification;
    private readonly ILogger<ArticleStatusChangedNotificationHandler> _logger;

    public ArticleStatusChangedNotificationHandler(INotificationService notification,
        IDateTimeProvider timeProvider,
        ILogger<ArticleStatusChangedNotificationHandler> logger)
    {
        _notification = notification;
        _timeProvider = timeProvider;
        _logger = logger;
    }
    
    public async Task Handle(ArticleStatusChangedNotification request, CancellationToken cancellationToken)
    {
        if ((_timeProvider.Utc - request.Time).Minutes > 5)
        {
            _logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await _notification.NotifyAll(TrendNotifications.ArticleStatusChanged);
    }
}
