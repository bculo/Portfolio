using System.Net;
using FluentAssertions;
using Http.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

namespace Trend.IntegrationTests.SearchWord;

internal static class Endpoints
{
    public const string Filter = "/api/v1/searchword/Filter";
}

public class SearchWordControllerTests(TrendApiFactory factory) : TrendControllerBaseTest(factory)
{
    [Fact] 
    public async Task Filter_ShouldReturnOk_WhenRequestObjectValid()
    {
        Client.WithRole(UserRole.User);
        
        var request = new FilterSearchWordsReqDto
        {
            Active = ActiveFilter.Active.Id,
            Page = 1,
            Query = string.Empty,
            ContextType = ContextType.All.Value,
            Sort = SortType.Asc.Value,
            Take = 10,
            SearchEngine = SearchEngine.All
        };

        var queryParameters = request.AsQueryParametersString();
        
        //Act
        var response = await Client.GetAsync($"{Endpoints.Filter}?{queryParameters}");

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Filter_ShouldReturnBadRequest_WhenRequestObjectInvalid()
    {
        Client.WithRole(UserRole.User);
        var request = new FilterSearchWordsReqDto
        {
            Active = 300,
            Page = -1,
            Query = null,
            ContextType = 201,
            Sort = 123,
            Take = -5,
            SearchEngine = 90
        };
        var queryParameters = request.AsQueryParametersString();
        
        var response = await Client.GetAsync($"{Endpoints.Filter}?{queryParameters}");
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.ExtractContentFromResponse<HttpValidationProblemDetails>();
        body.Status.Should().Be((int)HttpStatusCode.BadRequest);
        body.Errors["Active"].Should().HaveCountGreaterThan(0);
        body.Errors["Page"].Should().HaveCountGreaterThan(0);
        body.Errors["ContextType"].Should().HaveCountGreaterThan(0);
        body.Errors["Sort"].Should().HaveCountGreaterThan(0);
        body.Errors["Take"].Should().HaveCountGreaterThan(0);
        body.Errors["SearchEngine"].Should().HaveCountGreaterThan(0);
    }
}