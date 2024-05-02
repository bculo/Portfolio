namespace Events.Common.Crypto
{
    public class AddItemTimeoutExpired
    {
        public Guid CorrelationId { get; set; }
        public string Symbol { get; set; } = default!;
    }
}
