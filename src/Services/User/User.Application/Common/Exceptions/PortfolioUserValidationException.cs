using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Common.Exceptions
{
    public class PortfolioUserValidationException : PortfolioUserCoreException
    {
        public IDictionary<string, string[]> Errors { get; private set; }

        public PortfolioUserValidationException(IDictionary<string, string[]> errors)
            : base("Validation exception occurred", "Validation exception occurred")
        {
            Errors = errors;
        }
    }
}
