namespace Crypto.Shared.Utilities;

public static class SymbolGenerator
{
    public static string Generate()
    {
        return Guid.NewGuid().ToString().Replace("-", string.Empty);
    }
}