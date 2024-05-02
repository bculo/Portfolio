namespace Events.Common.Crypto
{
    public class AddItemWithDelay
    {
        public string Symbol { get; set; }  = default!;
        public Guid CorrelationId { get; set; }
    }
}
