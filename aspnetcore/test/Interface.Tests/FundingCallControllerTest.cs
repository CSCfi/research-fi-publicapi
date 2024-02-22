using System;
using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Controllers;
using CSC.PublicApi.Interface.Maps;
using CSC.PublicApi.Interface.Services;
using CSC.PublicApi.Service.Models.FundingCall;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ResearchFi.Query;
using Xunit;
using Serilog;

namespace CSC.PublicApi.Interface.Tests;

public class FundingCallControllerTest : IDisposable
{
    private readonly FundingCallController _controller;
    private readonly Mock<ISearchService<FundingCallSearchParameters, FundingCall>> _mockSearchService;
    private readonly IFundingCallService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public FundingCallControllerTest()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var controllerLogger = loggerFactory.CreateLogger<FundingCallController>();
        var serviceLogger = loggerFactory.CreateLogger<FundingCallService>();

        _mockSearchService = new Mock<ISearchService<FundingCallSearchParameters, FundingCall>>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<FundingCallProfile>()).CreateMapper();
        _service = new FundingCallService(serviceLogger, mapper, _mockSearchService.Object);
        _diagnosticContext = new Mock<IDiagnosticContext>().Object;;

        _controller = new FundingCallController(controllerLogger, _service, _diagnosticContext);
    }

    [Fact]
    public void Get_PaginationWithSpecifiedParameters_IsSuccessful()
    {
        // Arrange
        const int pageNumber = 7;
        const int pageSize = 50;
        var queryParameters = new GetFundingCallQueryParameters { PageSize = pageSize, PageNumber = pageNumber };

        // Act
        var result = _controller.Get(queryParameters);

        // Assert
        result.Should().NotBeNull();
        _mockSearchService.Verify(m => m.Search(It.IsAny<FundingCallSearchParameters>(), pageNumber, pageSize));
    }

    [Fact]
    public void Get_PaginationWithNegativeValues_UseOneInstead()
    {
        // Arrange
        const int pageNumber = -1;
        const int pageSize = -5;
        var queryParameters = new GetFundingCallQueryParameters { PageSize = pageSize, PageNumber = pageNumber };

        // Act
        var result = _controller.Get(queryParameters);

        // Assert
        result.Should().NotBeNull();
        _mockSearchService.Verify(m => m.Search(It.IsAny<FundingCallSearchParameters>(), 1, 1));
    }

    [Fact]
    public void Get_PaginationWithNoSpecifiedParameters_UsesDefaultValues()
    {
        // Arrange
        const int defaultPageNumber = 1;
        const int defaultSize = 20;

        // Act
        var result = _controller.Get(new GetFundingCallQueryParameters());

        // Assert
        result.Should().NotBeNull();
        _mockSearchService.Verify(m => m.Search(It.IsAny<FundingCallSearchParameters>(), defaultPageNumber, defaultSize));
    }

    [Fact]
    public void Get_PaginationWithTooLargePageSize_SetsPageSizeToMaximum()
    {
        // Arrange
        const int maximumPageSize = 100;
        const int pageNumber = 7;
        const int pageSize = 150;
        var queryParameters = new GetFundingCallQueryParameters { PageSize = pageSize, PageNumber = pageNumber };

        // Act
        var result = _controller.Get(queryParameters);

        // Assert
        result.Should().NotBeNull();
        _mockSearchService.Verify(m => m.Search(It.IsAny<FundingCallSearchParameters>(), pageNumber, maximumPageSize));
    }

    public void Dispose()
    {
    }
}