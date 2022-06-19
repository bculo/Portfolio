using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Queries.Responses.Common
{
    public class PageResponse<T>
    {
        public long Count { get; private set; }
        public List<T> Items { get; private set; }

        public PageResponse(long count, List<T> items)
        {
            Count = count;
            Items = items;
        }
    }
}
