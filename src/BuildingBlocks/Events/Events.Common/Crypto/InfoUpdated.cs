namespace Events.Common.Crypto
{
    public class InfoUpdated
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }  = default!;
        public string Name { get; set; }  = default!;
        public string? Website { get; set; }
        public string Description { get; set; }  = default!;
    }
}
