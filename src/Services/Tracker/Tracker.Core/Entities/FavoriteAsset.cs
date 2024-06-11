using Tracker.Core.Enums;

namespace Tracker.Core.Entities;

public class FavoriteAsset
{
    public long Id { get; set; }
    public string Symbol { get; set; }
    public FinancialAssetType AssetType { get; set; }
    public Guid UserId { get; set; }
}