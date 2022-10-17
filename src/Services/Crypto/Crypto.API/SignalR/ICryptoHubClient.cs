namespace Crypto.API.SignalR
{
    public interface ICryptoHubClient
    {
        Task ReceiveCryptoPriceUpdateNotificaiton(string message);
    }
}
