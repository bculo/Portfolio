using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Domain.Exceptions
{
    public class TrendValidationException : TrendAppCoreException
    {
        public string Title => "One or more validation errors occurred.";
        public Dictionary<string, List<string>> Errors { get; set; }

        public TrendValidationException(Dictionary<string, List<string>> errors) 
            : base("Validation exception")
        {
            Errors = errors;
        }

        public TrendValidationException(Dictionary<string, List<string>> errors, string message)
            : base(message)
        {
            Errors = errors;
        }
    }
}
