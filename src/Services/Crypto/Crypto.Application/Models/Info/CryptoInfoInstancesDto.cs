using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Application.Models.Info
{
    public class CryptoInfoResponseDto
    {
        [JsonPropertyName("status")]
        public CryptoInfoStatusDto Status { get; set; }
        [JsonPropertyName("data")]
        public Dictionary<string, List<CryptoInfoDataDto>> Data { get; set; }
    }

    public class CryptoInfoStatusDto
    {
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }
        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }
        [JsonPropertyName("notice")]
        public string Notice { get; set; }
    }

    public class CryptoInfoDataDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("logo")]
        public string Logo { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        //[JsonPropertyName("urls")]
        //public Dictionary<string, string[]> Urls { get; set; }
    }
}
