using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;

namespace Trend.Domain.Queries.Requests.News
{
    public class SearchArticleRequestQuery
    {
        public ContextType Type { get; set; }
    }
}
