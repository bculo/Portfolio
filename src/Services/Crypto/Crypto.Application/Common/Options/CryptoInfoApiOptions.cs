namespace Crypto.Application.Common.Options
{
    public sealed class CryptoInfoApiOptions
    {
        public string HeaderKey { get; init; } = default!;
        public string ApiKey { get; init; } = default!;
        public string BaseUrl { get; init; } = default!;
    }
}
