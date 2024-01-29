using Stock.Application.Interfaces.Html.Models;

namespace Stock.Application.Interfaces.Html
{
    public interface IHtmlParser
    {
        Task<bool> Initialize(string htmlContent);
        Task<IEnumerable<HtmlNodeElement>> FindElements(string xPathQuery);
        Task<HtmlNodeElement> FindFirstElement(string xPathQuery);
    }
}
