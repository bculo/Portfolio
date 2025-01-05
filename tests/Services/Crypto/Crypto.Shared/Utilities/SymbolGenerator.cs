namespace Crypto.Shared.Utilities;

public static class SymbolGenerator
{
    public static string Generate()
    {
        return Ulid.NewUlid().ToString();
    }
}