﻿using Events.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class CryptoVisited : IntegrationEvent
    {
        public long CryptoId { get; set; }
        public string Symbol { get; set; }
    }
}
