using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Domain.Exceptions
{
    public class TrendAppAuthenticationException : TrendAppCoreException
    {
        public TrendAppAuthenticationException(string message) : base(message)
        {

        }
    }
}
