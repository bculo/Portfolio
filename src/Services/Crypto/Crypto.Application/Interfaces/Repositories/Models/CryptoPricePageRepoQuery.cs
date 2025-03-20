namespace Crypto.Application.Interfaces.Repositories.Models;

public record CryptoPricePageRepoQuery(string? Symbol, int Page, int Take) : PageRepoQuery(Page, Take);