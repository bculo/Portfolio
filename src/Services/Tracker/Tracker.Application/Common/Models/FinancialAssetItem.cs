using MassTransit.Testing;
using Tracker.Core.Enums;

namespace Tracker.Application.Common.Models;

public class FinancialAssetItem
{
    public FinancialAssetType Type { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}