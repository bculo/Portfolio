using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Stock.API.IntegrationTests.Data;
using Stock.Application.Commands.Stock;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Stock.API.IntegrationTests.Stock;

public class CreateStockTests : StockControllerBaseTest
{
    public CreateStockTests(StockApiFactory factory) : base(factory)
    {
    }
    
    [Theory]
    [InlineData(" ")]
    [InlineData("123")]
    public async Task ShouldReturnBadRequest_WhenInvalidSymbolProvided(string symbol)
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        var request = new CreateStock(symbol);
        
        //Act
        var response = await Client.PostAsync("/api/v1/stock/create", request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Type.Should().Be("Validation.Error");
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenSymbolAlreadyExists()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        
        string symbol = "TSLA";
        _ = await Helper.Create(symbol);
        
        var request = new CreateStock(symbol);
        
        //Act
        var response = await Client.PostAsync("/api/v1/stock/create", request.AsHttpContent());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Type.Should().Be(StockErrorCodes.STOCK_DUPLICATE);
    }

    
    [Fact]
    public async Task ShouldReturnStatusOk_WhenProvidedSymbolIsLegit()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);

        string symbol = "TSLA";
        await Factory.MockServer.ReturnsWithTextOk(new MarketDataLoader());
        var request = new CreateStock(symbol);
        
        //Act
        var response = await Client.PostAsync("/api/v1/stock/create", request.AsHttpContent());

        response.EnsureSuccessStatusCode();
    }
}