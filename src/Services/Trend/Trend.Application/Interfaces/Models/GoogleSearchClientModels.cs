using Newtonsoft.Json;

namespace Trend.Application.Interfaces.Models;

public class GoogleSearchEngineItemDto
{
    [JsonProperty("kind")]
    public string Kind { get; set; } = default!;
    [JsonProperty("title")]
    public string Title { get; set; } = default!;
    [JsonProperty("htmlTitle")]
    public string HtmlTitle { get; set; } = default!;
    [JsonProperty("link")]
    public string Link { get; set; } = default!;
    [JsonProperty("snippet")]
    public string Snippet { get; set; } = default!;
    [JsonProperty("displayLink")]
    public string DisplayLink { get; set; } = default!;
    [JsonProperty("pagemap")]
    public GoogleSearchEnginePageMapDto PageMap { get; set; } = default!;
}

public class GoogleSearchEngineMetatagDto
{
    [JsonProperty("article:author")]
    public string Author { get; set; } = default!;
}

public class GoogleSearchEnginePageMapDto
{
    [JsonProperty("cse_thumbnail")]
    public List<GoogleSearchEngineThumbnailDto> Thumbnails { get; set; } = default!;
    [JsonProperty("metatags")]
    public List<GoogleSearchEngineMetatagDto> Metatags { get; set; } = default!;
}

public class GoogleSearchEngineRequestInformationDto
{
    [JsonProperty("totalResults")]
    public long TotalResults { get; set; } 
}

public class GoogleSearchEngineResponseDto
{
    [JsonProperty("kind")]
    public string Kind { get; set; } = default!;

    [JsonProperty("searchInformation")]
    public GoogleSearchEngineRequestInformationDto SearchInformation { get; set; } = default!;

    [JsonProperty("items")]
    public List<GoogleSearchEngineItemDto> Items { get; set; } = default!;
}

public class GoogleSearchEngineThumbnailDto
{
    [JsonProperty("src")]
    public string Src { get; set; } = default!;

    [JsonProperty("width")]
    public int Width { get; set; } = default!;

    [JsonProperty("height")]
    public int Height { get; set; } = default!;
}