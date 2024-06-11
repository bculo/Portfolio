using Tracker.Core.Enums;

namespace Tracker.Application.Interfaces.Integration;

public interface IFinancialAssetAdapterFactory
{
    IFinancialAssetAdapter GetAdapter(FinancialAssetType type);
}