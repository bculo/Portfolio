using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Functions.Models
{
    public class FailureResponse<T> where T : notnull
    {
        public T Message { get; set; }
        public int AppCode { get; set; }
    }
}
