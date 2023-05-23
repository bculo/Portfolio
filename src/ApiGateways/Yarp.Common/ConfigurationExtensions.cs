using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Yarp.Common.Options;

namespace Yarp.Common;

public static class ConfigurationExtensions
{
    public static void ConfigureYarpProxy(this WebApplicationBuilder builder, string reverseProxySection = "ReverseProxy")
    {
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection(reverseProxySection));

        builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(nameof(ApplicationOptions)));
    }
}