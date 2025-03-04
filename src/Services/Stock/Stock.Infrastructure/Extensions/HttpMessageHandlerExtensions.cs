using Microsoft.Extensions.DependencyInjection;

namespace Stock.Infrastructure.Extensions;

public static class HttpMessageHandlerExtensions
{
    public static IHttpClientBuilder ApplyHttpMessageHandler<T>(
        this IHttpClientBuilder builder,
        bool isDevelopment) where T : HttpMessageHandler
    {
        return isDevelopment ? builder.ConfigurePrimaryHttpMessageHandler<T>() : builder;
    }
}