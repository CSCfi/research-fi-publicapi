using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Controllers;
using CSC.PublicApi.Interface.Maps.V1;
using CSC.PublicApi.Interface.Services.V1;
using CSC.PublicApi.Service.Models.FundingCall;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ResearchFi.Query;
using Serilog;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class FundingCallControllerTest
{
    private readonly Mock<ISearchService<FundingCallSearchParameters, FundingCall>> _mockSearchService = new();
    private readonly FundingCallController _controller;
    private readonly DefaultHttpContext _httpContext = new();

    public FundingCallControllerTest()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<FundingCallProfile>()).CreateMapper();
        var service = new FundingCallService(
            NullLogger<FundingCallService>.Instance,
            mapper,
            _mockSearchService.Object);
        _controller = new FundingCallController(
            NullLogger<FundingCallController>.Instance,
            service,
            new Mock<IDiagnosticContext>().Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContext }
        };
        _httpContext.Request.Scheme = "https";
        _httpContext.Request.Host = new HostString("api.test");
        _httpContext.Request.Path = "/v1/funding-calls";
        _httpContext.Request.QueryString = new QueryString("?source=test");
    }

    [Theory]
    [InlineData(7, 50, 7, 50)]
    [InlineData(-1, -5, 1, 1)]
    [InlineData(1, 20, 1, 20)]
    [InlineData(7, 150, 7, 100)]
    public async Task Get_Pagination_UsesNormalizedParametersAndReturnsResultsAndHeaders(
        int requestedPageNumber,
        int requestedPageSize,
        int expectedPageNumber,
        int expectedPageSize)
    {
        var expectedFundingCall = new FundingCall
        {
            NameFi = "Synthetic funding call"
        };
        var expectedSearchResult = new SearchResult(expectedPageNumber, expectedPageSize, 123);
        _mockSearchService
            .Setup(service => service.Search(
                It.IsAny<FundingCallSearchParameters>(),
                expectedPageNumber,
                expectedPageSize))
            .ReturnsAsync((new[] { expectedFundingCall }, expectedSearchResult));

        var result = await _controller.Get(
            new GetFundingCallQueryParameters(),
            new PaginationQueryParameters
            {
                PageNumber = requestedPageNumber,
                PageSize = requestedPageSize
            });

        var fundingCalls = result.Should().ContainSingle().Subject;
        fundingCalls.NameFi.Should().Be(expectedFundingCall.NameFi);
        _httpContext.Response.Headers["x-current-page"].ToString().Should().Be(expectedPageNumber.ToString());
        _httpContext.Response.Headers["x-page-count"].ToString()
            .Should().Be(expectedSearchResult.TotalPages.ToString());
        _httpContext.Response.Headers["x-page-size"].ToString().Should().Be(expectedPageSize.ToString());
        _httpContext.Response.Headers["x-total"].ToString().Should().Be("123");
        _httpContext.Response.Headers["link"].ToString().Should().Contain("rel=\"last\"");
        _mockSearchService.Verify(
            service => service.Search(
                It.IsAny<FundingCallSearchParameters>(),
                expectedPageNumber,
                expectedPageSize),
            Times.Once);
    }

    [Fact]
    public async Task Get_WhenSearchFails_PropagatesTheDependencyFailure()
    {
        var exception = new InvalidOperationException("Synthetic search failure.");
        _mockSearchService
            .Setup(service => service.Search(
                It.IsAny<FundingCallSearchParameters>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .Returns(Task.FromException<(IEnumerable<FundingCall>, SearchResult)>(exception));

        var action = () => _controller.Get(
            new GetFundingCallQueryParameters(),
            new PaginationQueryParameters());

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Synthetic search failure.");
    }
}
