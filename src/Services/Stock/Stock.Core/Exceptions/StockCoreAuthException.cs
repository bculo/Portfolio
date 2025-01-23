namespace Stock.Core.Exceptions;

public class StockCoreAuthException(string message) : StockCoreException("Auth.Error", message);