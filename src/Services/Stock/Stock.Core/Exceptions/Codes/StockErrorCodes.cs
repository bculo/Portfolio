namespace Stock.Core.Exceptions.Codes;

public static class StockErrorCodes
{
    public static NotFoundErrorCode NotFoundBySymbol(string symbol) => 
        new("Stock.NotFoundBySymbol", $"Stock with symbol {symbol} not found");
    public static NotFoundErrorCode NotFoundById(string Id) => 
        new("Stock.NotFoundById", $"Stock with Id {Id} not found");
    public static ErrorCode Duplicate(string symbol) => 
        new("Stock.DuplicateSymbol", $"Stock with symbol {symbol} already exists");
    public static ErrorCode NotSupported(string symbol) => 
        new("Stock.NotSupportedSymbol", $"Stock with symbol {symbol} not supported by external provider");
}