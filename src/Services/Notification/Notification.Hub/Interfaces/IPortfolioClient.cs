namespace Notification.Hub.Interfaces
{
    public interface IPortfolioClient
    {
        Task Ping(string message);
    }
}
