using System.Net;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Interfaces;
using FluentAssertions;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.CryptoController;

public class GetPriceHistoryTests : BaseTests
{
    public GetPriceHistoryTests(CryptoApiFactory factory) 
        : base(factory)
    {
    }

    [Theory]
    [InlineData("BTC")]
    [InlineData("ETH")]
    public async Task GetAsync_ShouldReturnStatusOk_WhenExistingSymbolProvided(string symbol)
    {
        //Arrange
        _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
        
        //Act
        var response = await _client.GetAsync($"{ApiEndpoint.CRYPTO_PRICE_HISTORY}/{symbol}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("TROLL")]
    [InlineData("TDROP")]
    [InlineData("--")]
    public async Task GetAsync_ShouldReturnStatusNotFound_WhenExistingSymbolProvided(string symbol)
    {
        //Arrange
        _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
        
        //Act
        var response = await _client.GetAsync($"{ApiEndpoint.CRYPTO_PRICE_HISTORY}/{symbol}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}