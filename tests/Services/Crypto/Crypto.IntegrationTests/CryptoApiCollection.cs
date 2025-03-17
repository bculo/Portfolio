namespace Crypto.IntegrationTests;

[CollectionDefinition(nameof(CryptoApiCollection), DisableParallelization = false)]
public class CryptoApiCollection : ICollectionFixture<CryptoApiFactory>;

