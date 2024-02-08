using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Application.Common.Codes;
using Notification.Application.Interfaces.Notifications.Models;

namespace Notification.Application.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task NotifyAll(PushNotification notification);
        Task NotifyGroup(string groupId, PushNotification notification);
        Task NotifyUser(string userId, PushNotification notification);
    }
}
