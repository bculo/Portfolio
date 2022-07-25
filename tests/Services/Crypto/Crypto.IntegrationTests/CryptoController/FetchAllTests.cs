using Crypto.IntegrationTests.CryptoController.Constants;
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

        public FetchAllTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test()
        {
            //Arrange
            HttpClient client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(ApiEndpoint.FETCH_ALL);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
