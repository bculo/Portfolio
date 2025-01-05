namespace Crypto.Core.Entities
{
    public sealed class CryptoEntity : Entity
    {
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public string? WebSite { get; set; }
        public string? SourceCode { get; set; }
        public ICollection<VisitEntity> Visits { get; set; } = []; 
    }
}
