using System.Net;
using Microsoft.Extensions.Configuration;

namespace Stock.Infrastructure.Price;

public class ProxyHttpMessageHandler : HttpClientHandler
{
    public ProxyHttpMessageHandler(IConfiguration configuration)
    {
        var proxyAddress = configuration["MarketWatchOptions:ProxyAddress"];

        if (string.IsNullOrEmpty(proxyAddress))
        {
            throw new ArgumentException("Proxy address is null or empty");
        }

        Proxy = new WebProxy
        {
            Address = new Uri(proxyAddress)
        };
    }
}