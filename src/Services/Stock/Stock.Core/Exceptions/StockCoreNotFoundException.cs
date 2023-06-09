using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Exceptions
{
    public class StockCoreNotFoundException : StockCoreException
    {
        public StockCoreNotFoundException(string message)
            : base(message)
        {

        }

        public StockCoreNotFoundException(string userMessage, string devMessage)
            : base(userMessage, devMessage)
        {

        }
    }
}
