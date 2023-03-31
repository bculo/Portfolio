using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAll(string message);
        Task NotifyAll(object message);

        Task NotifyGroup(string groupId, string message);
        Task NotifyGroup(string groupId, object message);

        Task NotifyUser(string userId, string message);
        Task NotifyUser(string userId, object message);
    }
}
