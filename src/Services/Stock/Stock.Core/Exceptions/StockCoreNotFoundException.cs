using Stock.Core.Exceptions.Codes;

namespace Stock.Core.Exceptions
{
    public class StockCoreNotFoundException : StockCoreException
    {
        public object[] Args { get; set; }
        
        public StockCoreNotFoundException(NotFoundErrorCode errorCode)
            : base(errorCode.Code, errorCode.Message)
        {
        }
    }
}
