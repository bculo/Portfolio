namespace Crypto.IntegrationTests
{
    [CollectionDefinition(nameof(CryptoApiCollection), DisableParallelization = true)]
    public class CryptoApiCollection : ICollectionFixture<CryptoApiFactory>
    {
    }
}
