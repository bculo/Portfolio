namespace Trend.Domain.Exceptions;

public class TrendAppAuthenticationException : TrendAppCoreException
{
    public TrendAppAuthenticationException(string message) : base(message)
    {

    }
}