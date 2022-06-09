using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Time.Common
{
    public class UtcDateTimeService : IDateTime
    {
        public DateTime DateTime => DateTime.UtcNow;
    }
}
