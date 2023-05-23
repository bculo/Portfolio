namespace Tracker.Application.Interfaces;

public interface IFinancialAssetClient
{
    Task<FinancialAssetDto> FetchAsset(string symbol);
}

public class FinancialAssetDto
{
    public string Symbol { get; set; }
    public float Price { get; set; }
    public string Name { get; set; }
}