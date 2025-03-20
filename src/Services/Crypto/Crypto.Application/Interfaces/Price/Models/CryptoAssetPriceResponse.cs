namespace Crypto.Application.Interfaces.Price.Models;

public record CryptoAssetPriceResponse(string Symbol, string Currency, decimal Price);