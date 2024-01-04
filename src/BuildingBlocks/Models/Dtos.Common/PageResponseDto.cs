using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common
{
    public class PageResponseDto<T>
    {
        public long Count { get; init; }
        public List<T> Items { get; init; }
    }
}
