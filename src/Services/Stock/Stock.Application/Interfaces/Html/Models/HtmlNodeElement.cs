namespace Stock.Application.Interfaces.Html.Models;

public class HtmlNodeElement
{
    public string Text { get; set; } = null!;
    public Dictionary<string, string> Attributes { get; set; }  = null!;
    public string HtmlElementType { get; set; }  = null!;
    public string Html { get; set; }  = null!;
}