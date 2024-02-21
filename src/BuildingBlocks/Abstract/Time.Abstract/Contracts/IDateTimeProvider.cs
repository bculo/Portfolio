using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time.Abstract.Contracts
{
    public interface IDateTimeProvider
    {
        public DateTime Now { get; }
        public DateTime Utc { get; } 
        public DateTimeOffset Offset { get; }
        public DateTimeOffset UtcOffset { get; }
    }
}
