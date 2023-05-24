using Tracker.Application.Enums;

namespace Tracker.Application.Options;

public sealed class FinancalAssetClientFactoryOptions
{
    public ServiceCommunicationType CryptoService { get; set; }
    public ServiceCommunicationType StockService { get; set; }
}
