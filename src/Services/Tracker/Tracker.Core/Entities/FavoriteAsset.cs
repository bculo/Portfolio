using Tracker.Core.Enums;

namespace Tracker.Core.Entities;

public class FavoriteAsset
{
    public long Id { get; set; }
    public string Symbol { get; set; }
    public FinancalAssetType AssetType { get; set; }
    public string UserId { get; set; }
}