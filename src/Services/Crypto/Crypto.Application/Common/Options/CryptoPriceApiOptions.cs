namespace Crypto.Application.Common.Options
{
    public sealed class CryptoPriceApiOptions
    {
        public string HeaderKey { get; init; } = default!;
        public string ApiKey { get; init; } = default!;
        public string BaseUrl { get; init; } = default!;
        public string Currency { get; init; } = default!;
    }
}
