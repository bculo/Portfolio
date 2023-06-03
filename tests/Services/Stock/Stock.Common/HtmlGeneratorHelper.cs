using HtmlAgilityPack;

namespace Stock.Common
{
    public class HtmlGeneratorHelper
    {
        private const string HTML_CONTENT = @"
                <!DOCTYPE html>
                <html>
                    <head>
                        <title>Page Title</title>
                    </head>
                    <body>
                    
                    </body>
                </html>
        ";

        private HtmlDocument _document;

        public HtmlGeneratorHelper()
        {
            _document = new HtmlDocument();
            _document.LoadHtml(HTML_CONTENT);
        }

        public void InsertAdditionalContent(string text, string htmlElement)
        {
            var node = _document.DocumentNode.SelectSingleNode("//body");
            var newNode = _document.CreateElement(htmlElement);
            newNode.InnerHtml = text;
            node.AppendChild(newNode);
        }

        public void InsertAdditionalContent(string text, string className, string htmlElement)
        {
            var node = _document.DocumentNode.SelectSingleNode("//body");
            var newNode = _document.CreateElement(htmlElement);
            newNode.InnerHtml = text;
            newNode.AddClass(className);
            node.AppendChild(newNode);
        }

        public HtmlDocument Build()
        {
            return _document;
        }
    }
}
