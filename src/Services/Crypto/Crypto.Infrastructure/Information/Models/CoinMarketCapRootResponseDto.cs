using System.Text.Json.Serialization;

namespace Crypto.Infrastructure.Information.Models;

public class CoinMarketCapRootResponseDto
{
    [JsonPropertyName("status")] public CoinMarketCapInfoStatusDto Status { get; set; } = default!;
    [JsonPropertyName("data")] public Dictionary<string, List<CoinMarketCapInfoDto>> Data { get; set; } = default!;
}

public class CoinMarketCapInfoStatusDto
{
    [JsonPropertyName("timestamp")] public DateTime TimeStamp { get; set; }
    [JsonPropertyName("error_code")] public int ErrorCode { get; set; }
    [JsonPropertyName("error_message")] public string? ErrorMessage { get; set; }
    [JsonPropertyName("notice")] public string? Notice { get; set; }
}

public class CoinMarketCapInfoDto
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = default!;
    [JsonPropertyName("symbol")] public string Symbol { get; set; } = default!;
    [JsonPropertyName("category")] public string Category { get; set; } = default!;
    [JsonPropertyName("logo")] public string? Logo { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = default!;
    [JsonPropertyName("urls")] public Dictionary<string, string[]> Urls { get; set; } = default!;
}