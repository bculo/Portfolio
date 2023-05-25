using MassTransit.Testing;
using Tracker.Core.Enums;

namespace Tracker.Application.Models;

public class FinancialAssetItem
{
    public FinancalAssetType Type { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}