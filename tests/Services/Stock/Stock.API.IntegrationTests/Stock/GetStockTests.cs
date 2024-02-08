using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Queries.Stock;
using Stock.Core.Exceptions.Codes;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Stock.API.IntegrationTests.Stock;

public class GetStockTests : StockControllerBaseTest
{
    public GetStockTests(StockApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task ShouldReturnStatusNotFound_WhenItemWithGivenIdDoesntExist()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        string id = "asd123basd";
        
        //Act
        var response = await Client.GetAsync($"/api/v1/stock/single/{id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Type.Should().Be(StockErrorCodes.STOCK_NOT_FOUND_BY_ID);
    }
    
    [Fact]
    public async Task ShouldReturnStatusOk_WhenItemExists()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        var symbol = "TSLA";
        var (_, id) = await Helper.CreateWithEncodedId(symbol);
        
        //Act
        var response = await Client.GetAsync($"/api/v1/stock/single/{id}");

        //Assert
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<GetStockByIdResponse>();
        item!.Symbol.Should().Be(symbol);
        item!.Id.Should().Be(id);
    }
}