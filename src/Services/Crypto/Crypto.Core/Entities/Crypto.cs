namespace Crypto.Core.Entities
{
    public sealed class Crypto : Entity
    {
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Logo { get; set; }
        public string? WebSite { get; set; }
        public string? SourceCode { get; set; }
        public ICollection<Visit> Visits { get; set; } = []; 
    }
}
