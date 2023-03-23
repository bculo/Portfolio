namespace Notification.Hub.Interfaces
{
    public interface ISignalRClient
    {
        Task Ping(string message);
    }
}
