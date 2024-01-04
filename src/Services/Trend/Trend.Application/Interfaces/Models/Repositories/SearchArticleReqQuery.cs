using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces.Models.Repositories
{
    public class SearchArticleReqQuery
    {
        public ContextType Type { get; set; }
    }
}
