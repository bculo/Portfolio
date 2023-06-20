using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class HtmlParserService : IHtmlParser
    {
        private readonly ILogger<HtmlParserService> _logger;

        public HtmlParserService(ILogger<HtmlParserService> logger, HtmlDocument document)
        {
            _logger = logger;
            _document = document;

            if (document is not null)
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
            if (!_initialized)
            {
                return Task.FromResult(false);
            }

            _document = new HtmlDocument();
            _document.LoadHtml(htmlContent);

            if (_document.ParseErrors.Any())
            {
                _logger.LogTrace("Error occured in process of parsing string to HTML content");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<IEnumerable<HtmlNodeElement>> FindElements(string xPathQuery)
        {
            if (string.IsNullOrWhiteSpace(xPathQuery) || !_initialized)
            {
                return Task.FromResult(Enumerable.Empty<HtmlNodeElement>());
            }

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

        public Task<HtmlNodeElement> FindFirstElement(string xPathQuery)
        {
            if (string.IsNullOrWhiteSpace(xPathQuery) || !_initialized)
            {
                return default;
            }

            var node = _document.DocumentNode.SelectNodes(xPathQuery)?.FirstOrDefault();
            if (node is null)
            {
                return null;
            }

            return Task.FromResult(new HtmlNodeElement
            {
                Text = node.InnerHtml,
                Attributes = node.Attributes.ToDictionary(x => x.Name, y => y.Value),
                HtmlElementType = node.OriginalName,
                Html = node.OuterHtml
            });
        }
    }
}
