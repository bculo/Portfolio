namespace Stock.Application.Common.Options
{
    public sealed class BatchUpdateOptions
    {
        public int BatchSize { get; set; }
        public bool IgnoreExchangeActiveTime { get; set; }
    }
}
