using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Features.Trend.Codes;

public static class TrendNotifications
{
    public static readonly PushNotification SyncExecuted = new("Trend.SyncExecuted", "Sync executed.");
    public static readonly PushNotification ArticleStatusChanged 
        = new("Trend.ArticleStatusChanged", "Article status changed");
}