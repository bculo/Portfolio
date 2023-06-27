using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Common.Exceptions
{
    public class PortfolioUserNotFoundException : PortfolioUserCoreException
    {
        public PortfolioUserNotFoundException(string userMessage)
            : base("Item not found", userMessage)
        {
            
        }
    }
}
