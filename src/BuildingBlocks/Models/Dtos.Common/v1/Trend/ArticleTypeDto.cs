using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend
{
    public class ArticleTypeDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string PageSource { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
    }
}
