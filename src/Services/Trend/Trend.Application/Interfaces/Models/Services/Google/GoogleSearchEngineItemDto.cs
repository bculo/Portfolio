using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Services.Google
{
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
}
