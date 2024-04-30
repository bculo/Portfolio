using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Features.Trend.Types;

public static class TrendNotifications
{
    public static readonly PushNotification SyncExecuted = new("Trend.SyncExecuted", "Sync executed.");
    public static readonly PushNotification ArticleStatusChanged 
        = new("Trend.ArticleStatusChanged", "Article status changed");
    
    public static readonly PushNotification SearchWordStatusChanged 
        = new("Trend.SearchWordStatusChanged", "Search word status changed");
}