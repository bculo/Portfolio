using Stock.Core.Exceptions.Codes;

namespace Stock.Core.Exceptions
{
    public class StockCoreException : Exception
    {
        public string Code { get; }

        protected StockCoreException(string errorIdentifier, string message)
            : base(message)
        {
            Code = errorIdentifier;
        }
        
        public StockCoreException(ErrorCode errorCode)
            : this(errorCode.Code, errorCode.Message)
        {
        }
    }
}

