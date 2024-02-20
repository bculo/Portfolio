namespace Crypto.Application.Interfaces.Information.Models;

public class CryptoInformation
{
    public string Symbol { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string? SourceCode { get; set; }
}