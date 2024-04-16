using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Options
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string InstanceName { get; set; } = default!;
        public int ExpirationTime { get; set; }
    }
}
