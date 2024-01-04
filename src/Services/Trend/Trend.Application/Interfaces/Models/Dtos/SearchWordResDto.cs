using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public class SearchWordResDto
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string SearchWord { get; set; }
        public string SearchEngineName { get; set; }
        public int SearchEngineId { get; set; }
        public string ContextTypeName { get; set; }
    }
}
