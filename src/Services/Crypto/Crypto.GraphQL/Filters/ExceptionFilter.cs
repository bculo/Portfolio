namespace Crypto.GraphQL.Filters
{
    public class ExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error;
        }
    }
}
