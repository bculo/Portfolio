using Newtonsoft.Json;

namespace Trend.Application.Interfaces.Models;

public class GoogleSearchEngineItemDto
{
    [JsonProperty("kind")]
    public string Kind { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("htmlTitle")]
    public string HtmlTitle { get; set; }
    [JsonProperty("link")]
    public string Link { get; set; }
    [JsonProperty("snippet")]
    public string Snippet { get; set; }
    [JsonProperty("displayLink")]
    public string DisplayLink { get; set; }
    [JsonProperty("pagemap")]
    public GoogleSearchEnginePageMapDto PageMap { get; set; }
}

public class GoogleSearchEngineMetatagDto
{
    [JsonProperty("article:author")]
    public string Author { get; set; }
}

public class GoogleSearchEnginePageMapDto
{
    [JsonProperty("cse_thumbnail")]
    public List<GoogleSearchEngineThumbnailDto> Thumbnails { get; set; }
    [JsonProperty("metatags")]
    public List<GoogleSearchEngineMetatagDto> Metatags { get; set; }
}

public class GoogleSearchEngineRequestInformationDto
{
    [JsonProperty("totalResults")]
    public  long TotalResults { get; set; }
}

public class GoogleSearchEngineResponseDto
{
    [JsonProperty("kind")]
    public string Kind { get; set; }

    [JsonProperty("searchInformation")]
    public GoogleSearchEngineRequestInformationDto SearchInformation { get; set; }

    [JsonProperty("items")]
    public List<GoogleSearchEngineItemDto> Items { get; set; }
}

public class GoogleSearchEngineThumbnailDto
{
    [JsonProperty("src")]
    public string Src { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }
}