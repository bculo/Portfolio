using Stock.Core.Errors;

namespace Stock.Core.Exceptions
{
    public class StockCoreNotFoundException : StockCoreException
    {
        public StockCoreNotFoundException(NotFoundErrorCode errorCode)
            : base(errorCode.Code, errorCode.Message)
        {
        }

        public static implicit operator StockCoreNotFoundException(NotFoundErrorCode errorCode) => new(errorCode);
    }
}
