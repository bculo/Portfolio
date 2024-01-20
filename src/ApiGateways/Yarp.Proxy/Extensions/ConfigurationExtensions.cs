using Yarp.Proxy.Common.Options;

namespace Yarp.Proxy.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureYarpProxy(this WebApplicationBuilder builder, string reverseProxySection = "ReverseProxy")
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection(reverseProxySection));

        builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(nameof(ApplicationOptions)));
    }
}