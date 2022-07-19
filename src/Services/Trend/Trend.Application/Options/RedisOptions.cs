using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Options
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int RememberTime { get; set; }
    }
}
