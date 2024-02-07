namespace Stock.Application.Interfaces.Html.Models;

public class HtmlNodeElement
{
    public string Text { get; set; } = default!;
    public Dictionary<string, string> Attributes { get; set; }  = default!;
    public string HtmlElementType { get; set; }  = default!;
    public string Html { get; set; }  = default!;
}