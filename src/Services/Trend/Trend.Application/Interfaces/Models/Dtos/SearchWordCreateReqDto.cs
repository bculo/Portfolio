using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public class SearchWordCreateReqDto
    {
        public string SearchWord { get; set; }
        public int SearchEngine { get; set; }
        public int ContextType { get; set; }
    }
}
