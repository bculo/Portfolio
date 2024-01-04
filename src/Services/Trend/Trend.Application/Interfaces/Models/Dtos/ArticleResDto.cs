using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public class ArticleResDto
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string PageSource { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string SearchWordId { get; set; }
        public string SearchWord { get; set; }
        public string SearchWordImage { get; set; }
    }
}
