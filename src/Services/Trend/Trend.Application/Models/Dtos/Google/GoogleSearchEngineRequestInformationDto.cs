using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Models.Dtos.Google
{
    public class GoogleSearchEngineRequestInformationDto
    {
        [JsonPropertyName("totalResults")]
        public  long TotalResults { get; set; }
    }
}
