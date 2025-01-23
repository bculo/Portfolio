namespace Stock.Core.Exceptions.Codes;

public static class StockErrorCodes
{
    public const string StockNotFoundBySymbol = "Stock.NotFoundBySymbol";
    public const string StockNotFoundById = "Stock.NotFoundById";
    public const string StockDuplicate = "Stock.DuplicateSymbol";
    public const string StockNotSupported = "Stock.NotSupportedSymbol";
    
    public static NotFoundErrorCode NotFoundBySymbol(string symbol) => 
        new(StockNotFoundBySymbol, $"Stock with symbol {symbol} not found");
    public static NotFoundErrorCode NotFoundById(string id) => 
        new(StockNotFoundById, $"Stock with Id {id} not found");
    public static ErrorCode Duplicate(string symbol) => 
        new(StockDuplicate, $"Stock with symbol {symbol} already exists");
    public static ErrorCode NotSupported(string symbol) => 
        new(StockNotSupported, $"Stock with symbol {symbol} not supported by external provider");
}