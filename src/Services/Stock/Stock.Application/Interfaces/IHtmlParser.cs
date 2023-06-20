using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Interfaces
{
    public interface IHtmlParser
    {
        Task<bool> InitializeHtmlContent(string htmlContent);
        Task<IEnumerable<HtmlNodeElement>> FindElements(string xPathQuery);
        Task<HtmlNodeElement> FindFirstElement(string xPathQuery);
    }

    public class HtmlNodeElement
    {
        public string Text { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public string HtmlElementType { get; set; }
        public string Html { get; set; }
    }
}
