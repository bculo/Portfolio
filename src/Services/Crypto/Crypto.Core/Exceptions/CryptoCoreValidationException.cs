using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Exceptions
{
    public class CryptoCoreValidationException : CryptoCoreException
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public CryptoCoreValidationException(Dictionary<string, string[]> errors) : base("Validation exception")
        {
            Errors = errors;
        }
    }
}
