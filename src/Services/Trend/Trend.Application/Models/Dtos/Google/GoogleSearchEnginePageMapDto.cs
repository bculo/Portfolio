using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Models.Dtos.Google
{
    public class GoogleSearchEnginePageMapDto
    {
        [JsonPropertyName("cse_thumbnail")]
        public List<GoogleSearchEngineThumbnailDto> Thumbnails { get; set; }
        [JsonPropertyName("metatags")]
        public List<GoogleSearchEngineMetatagDto> Metatags { get; set; }
    }
}
