namespace Crypto.Application.Interfaces.Repositories.Models;

public record CryptoPricePageQuery(string? Symbol, int Page, int Take) : PageQuery(Page, Take);