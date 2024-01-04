using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public record ArticleResDto
    {
        public string Id { get; init; }
        public DateTime Created { get; init; }
        public string Title { get; init; }
        public string Text { get; init; }
        public string Url { get; init; }
        public string PageSource { get; init; }
        public string TypeName { get; init; }
        public int TypeId { get; init; }
        public string SearchWordId { get; init; }
        public string SearchWord { get; init; }
        public string SearchWordImage { get; init; }
    }
}
