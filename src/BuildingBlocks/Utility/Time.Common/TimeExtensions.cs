using Microsoft.Extensions.DependencyInjection;
using Time.Abstract.Contracts;
using Time.Common.Services;

namespace Time.Common;

public static class TimeExtensions
{
    public static void AddUtcTimeProvider(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, UtcDateTimeService>();
    }
    
    public static void AddLocalTimeProvider(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
    }
}