﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Options
{
    public class CryptoPriceApiOptions
    {
        public string HeaderKey { get; set; }
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string[] Currencies { get; set; }
    }
}
