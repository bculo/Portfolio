using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Functions.Models
{
    public class FailureResponse
    {
        public object Message { get; set; } = default!;
        public int AppCode { get; set; }
    }
}
