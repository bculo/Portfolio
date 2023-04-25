using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests
{
    [CollectionDefinition("CryptoCollection")]
    public class CryptoSharedTestCollection : ICollectionFixture<CryptoApiFactory>
    {
    }
}
