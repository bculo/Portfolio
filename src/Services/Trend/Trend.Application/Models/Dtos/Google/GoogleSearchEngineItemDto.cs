using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Models.Dtos.Google
{
    public class GoogleSearchEngineItemDto
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("htmlTitle")]
        public string HtmlTitle { get; set; }
        public string Link { get; set; }
        [JsonPropertyName("snippet")]
        public string Snippet { get; set; }
        [JsonPropertyName("displayLink")]
        public string Page { get; set; }
        [JsonPropertyName("pagemap")]
        public GoogleSearchEnginePageMapDto PageMap { get; set; }
    }
}
