namespace Crypto.Application.Common.Options
{
    public sealed class QueueOptions
    {
        public string Prefix { get; init; } = default!;
        public string Address { get; init; } = default!;
        public bool Temporary { get; init; }
    }
}
