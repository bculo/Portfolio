using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Core.Errors;

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

