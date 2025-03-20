namespace Crypto.Application.Interfaces.Information.Models;

public record CryptoInfoDetailsResponse(
    string Symbol,
    string Name,
    string Description,
    string? Logo = null,
    string? Website = null,
    string? SourceCode = null
);