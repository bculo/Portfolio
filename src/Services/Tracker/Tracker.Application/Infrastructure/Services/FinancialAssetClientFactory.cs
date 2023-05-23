using Microsoft.Extensions.DependencyInjection;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.Application.Infrastructure.Services;

public class FinancialAssetClientFactory : IFinancialAssetClientFactory
{
    private readonly IServiceProvider _provider;

    public FinancialAssetClientFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IFinancialAssetClient CreateClient(FinancalAssetType type)
    {
        return type switch
        {
            FinancalAssetType.Crypto => _provider.GetRequiredService<CryptoAssetClient>(),
            _ => throw new NotSupportedException($"Financial asset client for type {type} not supported")
        };
    }
}