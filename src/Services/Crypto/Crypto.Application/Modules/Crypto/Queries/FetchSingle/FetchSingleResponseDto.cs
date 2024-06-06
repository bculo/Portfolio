namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleResponseDto
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
