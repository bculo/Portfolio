using Stock.Application.Interfaces.Html;

namespace Stock.Infrastructure.Html;

public class HtmlParserFactory : IHtmlParserFactory
{
    public IHtmlParser Create() => new HtmlParser();
}