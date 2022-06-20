using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.Shared
{
    public class PageRequestDto<T>
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public T? Search { get; set; }
    }

    public class PageRequestDto : PageRequestDto<object>
    {

    }
}
