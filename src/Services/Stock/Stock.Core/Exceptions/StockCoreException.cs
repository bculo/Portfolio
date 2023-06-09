using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Exceptions
{
    public class StockCoreException : Exception
    {
        public string UserMessage { get; set; }

        public StockCoreException(string message) : base(message)
        {
            UserMessage = message;
        }

        public StockCoreException(string userMessage, string devMessage) : base(devMessage)
        {
            UserMessage = userMessage;
        }

        public static void ThrowIfNull(object instance, string message)
        {
            if (instance is null)
            {
                throw new StockCoreException(message);
            }
        }

        public static void ThrowIfEmpty<TType>(IEnumerable<TType> values, string message)
        {
            if (values is null || !values.Any())
            {
                throw new StockCoreException(message);
            }
        }
    }
}
}
