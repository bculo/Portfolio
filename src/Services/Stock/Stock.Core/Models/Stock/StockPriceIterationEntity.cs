using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock;

public class StockPriceIterationEntity : Entity
{
    public DateTime Time { get; set; }
    public bool Finished { get; set; }
}