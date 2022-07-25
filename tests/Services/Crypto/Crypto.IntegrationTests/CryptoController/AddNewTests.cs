using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;
        private readonly HttpClient _client;

        public AddNewTests(CryptoApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }


    }
}
