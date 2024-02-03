using Microsoft.Extensions.DependencyInjection;

namespace Stock.gRPC.Client;

public static class ServicesExtensions
{
    public static void AddTrendGrpcClientFactory(this IServiceCollection services, string baseUrl)
    {
        services.AddGrpcClient<Greeter.GreeterClient>(opt =>
        {
            opt.Address = new Uri(baseUrl);
        });
        
        services.AddGrpcClient<Stock.StockClient>(opt =>
        {
            opt.Address = new Uri(baseUrl);
        });
    }
}