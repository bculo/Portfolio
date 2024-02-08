using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Common.Codes;

public static class TrendNotifications
{
    public static readonly PushNotification SyncExecuted = new("Trend.SyncExecuted", "Sync executed.");
}