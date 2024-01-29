namespace Stock.Application.Interfaces.Html.Models;

public class HtmlNodeElement
{
    public string Text { get; set; }
    public Dictionary<string, string> Attributes { get; set; }
    public string HtmlElementType { get; set; }
    public string Html { get; set; }
}