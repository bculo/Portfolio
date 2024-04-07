using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Crypto.Application.Interfaces.Information.Models;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Infrastructure.Information.Models;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewCryptoEndpointTests : BaseCryptoEndpointTests
    {

        
        public AddNewCryptoEndpointTests(CryptoApiFactory factory) 
            : base(factory)
        {
        }

        [Theory]
        [InlineData("BTC2")]
        [InlineData("---!-")]
        [InlineData("ETHIIIIIIIIIIIIIIIIIIIIIIIII")]
        public async Task ShouldReturnBadRequest_WhenInvalidSymbolProvided(string symbol)
        {
            //Arrange
            Client.WithRole(UserRole.Admin);
            var request = new AddNewCommand { Symbol = symbol };
        
            //Act
            var response = await Client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, request.AsHttpContent());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        
        [Theory]
        [InlineData("MATIC")]
        public async Task PostAsync_ShouldReturnStatusNoContent_WhenNonexistentValidSymbolProvided(string symbol)
        {
            //Arrange
            Client.WithRole(UserRole.Admin);
            var request = new AddNewCommand { Symbol = symbol };
            
            var infoResponse = MockFixture.Build<CoinMarketCapRootResponseDto>().Create();
            var firstItem = infoResponse.Data.Values!.FirstOrDefault();
            firstItem!.Insert(0, MockFixture.Build<CoinMarketCapInfoDto>()
                .With(x => x.Symbol, symbol)
                .Create());
            
            await Factory.MockServer.ReturnsWithJsonOk(infoResponse, withPath: "/info");

            //Act
            var response = await Client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, request.AsHttpContent());

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
