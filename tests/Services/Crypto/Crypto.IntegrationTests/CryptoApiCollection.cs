﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests
{
    [CollectionDefinition(nameof(CryptoApiCollection))]
    public class CryptoApiCollection : ICollectionFixture<CryptoApiFactory>
    {
    }
}
