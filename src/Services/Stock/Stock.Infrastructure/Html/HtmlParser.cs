using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Html.Models;

namespace Stock.Infrastructure.Html
{
    public class HtmlParser : IHtmlParser
    {
        public HtmlParser(HtmlDocument? document = null)
        {
            if (document == null) 
                return;
            
            _document = document;
            _initialized = true;
        }

        private bool _initialized;
        private HtmlDocument _document = null!;
        
        public Task<bool> Initialize(string htmlContent)
        {
            _initialized = !string.IsNullOrWhiteSpace(htmlContent);
            if (!_initialized)
                return Task.FromResult(false);

            _document = new HtmlDocument();
            _document.LoadHtml(htmlContent);

            if (_document.ParseErrors.Any())
                throw new ArgumentException("Invalid html content");
            
            return Task.FromResult(true);
        }

        public Task<IEnumerable<HtmlNodeElement>> FindElements(string xPathQuery)
        {
            if (string.IsNullOrWhiteSpace(xPathQuery) || !_initialized)
                return Task.FromResult(Enumerable.Empty<HtmlNodeElement>());

            var elements = _document.DocumentNode
                .SelectNodes(xPathQuery)
                .Where(i => i is not null)
                .Select(i => new HtmlNodeElement
                {
                    Text = i.InnerHtml,
                    Attributes = i.Attributes.ToDictionary(x => x.Name, y => y.Value),
                    HtmlElementType = i.OriginalName,
                    Html = i.OuterHtml,
                });

            return Task.FromResult(elements);
        }

        public Task<HtmlNodeElement?> FindFirstElement(string xPathQuery)
        {
            if (string.IsNullOrWhiteSpace(xPathQuery) || !_initialized)
                return null;

            var node = _document.DocumentNode.SelectNodes(xPathQuery)?.FirstOrDefault();
            if (node is null)
                return null;

            var nodeInfo = new HtmlNodeElement
            {
                Text = node.InnerHtml,
                Attributes = node.Attributes.ToDictionary(x => x.Name, y => y.Value),
                HtmlElementType = node.OriginalName,
                Html = node.OuterHtml
            };
            
            return Task.FromResult(nodeInfo)!;
        }
    }
}
