using Keycloak.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Keycloak.Common.Middlewares;

public class AccessTokenContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var service = context.RequestServices.GetService(typeof(IHttpRequestContextService)) as IHttpRequestContextService;
        service?.Init(context);
        await next(context);
    }
}

/// <summary>
/// Must be registered before Authentication and authorization middleware
/// </summary>
public static class HttpContextRequestMiddlewareExtensions
{
    public static IApplicationBuilder UseAccessTokenContextMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AccessTokenContextMiddleware>();
    }
}