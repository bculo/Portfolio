using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend.SearchWord
{
    public class SearchWordCreateDto
    {
        public string SearchWord { get; set; }
        public int SearchEngine { get; set; }
        public int ContextType { get; set; }
    }
}
