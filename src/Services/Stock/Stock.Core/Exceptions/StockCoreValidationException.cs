using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Exceptions
{
    public class StockCoreValidationException : StockCoreException
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public StockCoreValidationException(Dictionary<string, string[]> errors) 
            : base("Validation.Error", "One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
