using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.Trend.Codes;
using Notification.Application.Interfaces.Notifications;
using Time.Abstract.Contracts;

namespace Notification.Application.Features.Trend;

public record SearchWordStatusChangedNotification(string SearchWordId, DateTime Time) : IRequest;


public class SearchWordStatusChangedNotificationHandler : IRequestHandler<SearchWordStatusChangedNotification>
{
    private readonly IDateTimeProvider _timeProvider;
    private readonly INotificationService _notification;
    private readonly ILogger<SearchWordStatusChangedNotificationHandler> _logger;

    public SearchWordStatusChangedNotificationHandler(INotificationService notification,
        IDateTimeProvider timeProvider,
        ILogger<SearchWordStatusChangedNotificationHandler> logger)
    {
        _notification = notification;
        _timeProvider = timeProvider;
        _logger = logger;
    }
    
    public async Task Handle(SearchWordStatusChangedNotification request, CancellationToken cancellationToken)
    {
        if ((_timeProvider.Utc - request.Time).Minutes > 5)
        {
            _logger.LogWarning("Old sync event fetched");
            return;
        }
        
        await _notification.NotifyAll(TrendNotifications.SearchWordStatusChanged);
    }
}