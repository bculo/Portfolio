namespace Trend.Domain.Exceptions;

public class TrendNotFoundException : TrendAppCoreException
{
    public TrendNotFoundException() : base("Item not found")
    {

    }

    public TrendNotFoundException(string message) : base(message)
    {

    }
}