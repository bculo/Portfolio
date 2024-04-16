﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Options
{
    public sealed class CryptoPriceApiOptions
    {
        public string HeaderKey { get; set; } = default!;
        public string ApiKey { get; set; } = default!;
        public string BaseUrl { get; set; } = default!;
        public string Currency { get; set; } = default!;
    }
}
