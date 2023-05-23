using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Time.Common;
using Time.Common.Contracts;
using Tracker.Application.Infrastructure.Services;
using Tracker.Application.Interfaces;

namespace Tracker.Application;

public static class ApplicationLayer
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CryptoAssetClient>();
        services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
        services.AddScoped<IFinancialAssetClientFactory, FinancialAssetClientFactory>();
    }
}