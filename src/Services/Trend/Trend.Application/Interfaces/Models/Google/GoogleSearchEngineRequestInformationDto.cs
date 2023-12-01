using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Google
{
    public class GoogleSearchEngineRequestInformationDto
    {
        [JsonProperty("totalResults")]
        public  long TotalResults { get; set; }
    }
}
