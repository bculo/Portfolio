using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Options
{
    public sealed class CryptoInfoApiOptions
    {
        public string HeaderKey { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
