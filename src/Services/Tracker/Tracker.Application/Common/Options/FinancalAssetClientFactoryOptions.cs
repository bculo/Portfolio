using Tracker.Application.Common.Enums;

namespace Tracker.Application.Common.Options;

public sealed class FinancalAssetClientFactoryOptions
{
    public ServiceCommunicationType CryptoService { get; set; }
    public ServiceCommunicationType StockService { get; set; }
}
