using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Domain.Exceptions
{
    public class TrendNotFoundException : TrendAppCoreException
    {
        public TrendNotFoundException() : base("Item not found")
        {

        }

        public TrendNotFoundException(string message) : base(message)
        {

        }
    }
}
