using Tracker.Core.Enums;

namespace Tracker.Application.Interfaces;

public interface IFinancialAssetClientFactory
{
    IFinancialAssetClient CreateClient(FinancalAssetType type);
}