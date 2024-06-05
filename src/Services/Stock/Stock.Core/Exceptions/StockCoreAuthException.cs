namespace Stock.Core.Exceptions;

public class StockCoreAuthException : StockCoreException
{
    public StockCoreAuthException(string message) 
        : base("Auth.Error", message)
    {
    }
}