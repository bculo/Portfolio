using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.Shared
{
    public class PageResponseDto<T>
    {
        public long Count { get; set; }
        public List<T> Items { get; set; }
    }
}
