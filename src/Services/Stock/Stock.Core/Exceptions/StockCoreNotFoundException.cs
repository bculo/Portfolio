using Stock.Core.Exceptions.Codes;

namespace Stock.Core.Exceptions
{
    public class StockCoreNotFoundException(NotFoundErrorCode errorCode)
        : StockCoreException(errorCode.Code, errorCode.Message);
}
