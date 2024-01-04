using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Services.Google
{
    public class GoogleSearchEnginePageMapDto
    {
        [JsonProperty("cse_thumbnail")]
        public List<GoogleSearchEngineThumbnailDto> Thumbnails { get; set; }
        [JsonProperty("metatags")]
        public List<GoogleSearchEngineMetatagDto> Metatags { get; set; }
    }
}
