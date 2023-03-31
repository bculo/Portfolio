namespace Notification.Hub.Interfaces
{
    public interface ISignalRClient
    {
        Task Message(object message);
        Task Message(string message);
    }
}
