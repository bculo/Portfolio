﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Options
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int ExpirationTime { get; set; }
    }
}
