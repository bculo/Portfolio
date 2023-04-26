using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class DeleteTests : BaseTests
    {
        public DeleteTests(CryptoApiFactory factory) : base(factory)
        {
        }


    }
}
