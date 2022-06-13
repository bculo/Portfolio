using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Models.Dtos.Google
{
    public class GoogleSearchEngineResponseDto
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("searchInformation")]
        public GoogleSearchEngineRequestInformationDto SearchInformation { get; set; }

        [JsonProperty("items")]
        public List<GoogleSearchEngineItemDto> Items { get; set; }
    }
}
