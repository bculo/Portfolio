using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class FetchAllTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;
        private readonly HttpClient _client;

        public FetchAllTests(CryptoApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Test()
        {
            //Arrange
            HttpClient client = _factory.CreateClient();

            //Act
            var response = await _client.GetAsync("/api/crypto/fetchall");

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
