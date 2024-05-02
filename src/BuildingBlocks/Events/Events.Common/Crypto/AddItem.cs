namespace Events.Common.Crypto
{
    public class AddItem
    {
        public string Symbol { get; set; }  = default!;
        public Guid CorrelationId { get; set; }
    }
}
