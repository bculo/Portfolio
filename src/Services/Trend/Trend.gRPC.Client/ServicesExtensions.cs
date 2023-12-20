using Microsoft.Extensions.DependencyInjection;
using Trend.gRPC.Client.Protos.v1;

namespace Trend.gRPC.Client;

public static class ServicesExtensions
{
    public static void AddTrendGrpcClientFactory(this IServiceCollection services, string baseUrl)
    {
        services.AddGrpcClient<Greeter.GreeterClient>(ClientNamesConstants.GREETER, o =>
        {
            o.Address = new Uri(baseUrl);
        });
        
        services.AddGrpcClient<News.NewsClient>(ClientNamesConstants.NEWS, o =>
        {
            o.Address = new Uri(baseUrl);
        });
    }
}