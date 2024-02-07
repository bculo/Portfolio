namespace Stock.Core.Exceptions.Codes;

public static class StockErrorCodes
{
    public const string STOCK_NOT_FOUND_BY_SYMBOL = "Stock.NotFoundBySymbol";
    public const string STOCK_NOT_FOUND_BY_ID = "Stock.NotFoundById";
    public const string STOCK_DUPLICATE = "Stock.DuplicateSymbol";
    public const string STOCK_NOT_SUPPORTED = "Stock.NotSupportedSymbol";
    
    public static NotFoundErrorCode NotFoundBySymbol(string symbol) => 
        new(STOCK_NOT_FOUND_BY_SYMBOL, $"Stock with symbol {symbol} not found");
    public static NotFoundErrorCode NotFoundById(string id) => 
        new(STOCK_NOT_FOUND_BY_ID, $"Stock with Id {id} not found");
    public static ErrorCode Duplicate(string symbol) => 
        new(STOCK_DUPLICATE, $"Stock with symbol {symbol} already exists");
    public static ErrorCode NotSupported(string symbol) => 
        new(STOCK_NOT_SUPPORTED, $"Stock with symbol {symbol} not supported by external provider");
}