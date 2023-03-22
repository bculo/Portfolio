using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Exceptions
{
    public class CryptoCoreException : Exception
    {
        public string UserMessage { get; set; }

        public CryptoCoreException(string message) : base(message)
        {
            UserMessage = message;
        }

        public CryptoCoreException(string userMessage, string devMessage) : base(devMessage)
        {
            UserMessage = userMessage;
        }

        public static void ThrowIfNull(object instance, string message)
        {
            if (instance is null)
            {
                throw new CryptoCoreException(message);
            }
        }

        public static void ThrowIfEmpty<TType>(IEnumerable<TType> values, string message)
        {
            if(values is null || !values.Any())
            {
                throw new CryptoCoreException(message);
            }
        }
    }
}
