using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces;

namespace Stock.Application.Infrastructure.Services
{
    public class HtmlParserService : IHtmlParser
    {
        private readonly ILogger<HtmlParserService> _logger;

        public HtmlParserService(ILogger<HtmlParserService> logger, HtmlDocument document)
        {
            _logger = logger;
            _document = document;

            if(document is not null)
            {
                _initialized = true;
            }
        }

        private bool _initialized;
        private HtmlDocument _document;

        public string HtmlContent { get; private set; }

        public Task<bool> InitializeHtmlContent(string htmlContent)
        {
            _initialized = !string.IsNullOrWhiteSpace(htmlContent);
            if(!_initialized)
            {
                return Task.FromResult(false);
            }

            _document = new HtmlDocument();
            _document.LoadHtml(htmlContent);
            
            if(_document.ParseErrors.Any())
            {
                _logger.LogTrace("Error occured in process of parsing string to HTML content");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<IEnumerable<HtmlNodeElement>> FindElements(string xPathQuery)
        {
            if(string.IsNullOrWhiteSpace(xPathQuery) || !_initialized)
            {
                return Task.FromResult(Enumerable.Empty<HtmlNodeElement>());
            }

            var elements = _document.DocumentNode
                .SelectNodes(xPathQuery)
                .Select(i => new HtmlNodeElement
                {
                    Text = i.InnerHtml,
                    Attributes = i.Attributes.ToDictionary(x => x.Name, y => y.Value),
                    HtmlElementType = i.OriginalName
                });

            return Task.FromResult(elements);
        }
    }
}
