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
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("searchInformation")]
        public GoogleSearchEngineRequestInformationDto SearchInformation { get; set; }

        [JsonPropertyName("items")]
        public List<GoogleSearchEngineItemDto> Items { get; set; }
    }
}
