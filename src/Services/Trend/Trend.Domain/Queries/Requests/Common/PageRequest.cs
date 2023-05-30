using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Queries.Requests.Common
{
    public class PageRequest<T> 
    {
        public int Page { get; private set; }
        public int Take { get; private set; }
        public T Search { get; private set; }
        public int Skip => Page - 1;

        public PageRequest(int page, int take, T search)
        {
            Page = page;
            Take = take;
            Search = search;
        }
    }

    public class PageRequest : PageRequest<object>
    {
        public PageRequest(int page, int take) : base(page, take, null) 
        {

        }
    }
}
