using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Exceptions
{
    public class CryptoCoreNotFoundException : CryptoCoreException
    {
        public CryptoCoreNotFoundException(string message) : base(message)
        {

        }

        public CryptoCoreNotFoundException(string userMessage, string devMessage) : base(userMessage, devMessage)
        {

        }
        
        public static void ThrowIfNull(object instance, string message)
        {
            if (instance is null)
            {
                throw new CryptoCoreNotFoundException(message);
            }
        }
    }
}
