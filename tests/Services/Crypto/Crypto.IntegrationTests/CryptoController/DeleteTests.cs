using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.CryptoController
{
    public class DeleteTests : BaseTests
    {
        public DeleteTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task DeleteAsync_ShouldReturnStatusOk_WhenExistingSymbolProvided(string symbol)
        {
            //Arrange
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
        
            //Act
            var response = await _client.DeleteAsync($"{ApiEndpoint.CRYPTO_DELETE}/{symbol}");
        
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    
        [Theory]
        [InlineData("TROLL")]
        [InlineData("TDROP")]
        [InlineData("--")]
        public async Task DeleteAsync_ShouldReturnStatusNotFound_WhenExistingSymbolProvided(string symbol)
        {
            //Arrange
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
        
            //Act
            var response = await _client.DeleteAsync($"{ApiEndpoint.CRYPTO_DELETE}/{symbol}");
        
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
