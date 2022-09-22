using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Controllers;
using Api.DataAccess;
using Api.Models;
using Api.Models.FundingCall;
using Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace Api.Test
{
    public class FundingCallControllerTest : IDisposable
    {
        private readonly FundingCallController _controller;
        private readonly Mock<ISearchService<FundingCallSearchParameters, FundingCall>> _mockSearchService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public FundingCallControllerTest()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<FundingCallController>();

            _mockSearchService = new Mock<ISearchService<FundingCallSearchParameters, FundingCall>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _controller = new FundingCallController(logger, _mockSearchService.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public void Get_PaginationWithSpecifiedParameters_IsSuccessful()
        {
            // Arrange
            const int pageNumber = 7;
            const int pageSize = 50;
            var paginationParameters = new PaginationParameters { PageSize = pageSize, PageNumber = pageNumber };

            // Act
            var result = _controller.Get(new FundingCallSearchParameters(), paginationParameters);

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
            var paginationParameters = new PaginationParameters { PageSize = pageSize, PageNumber = pageNumber };

            // Act
            var result = _controller.Get(new FundingCallSearchParameters(), paginationParameters);

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
            var result = _controller.Get(new FundingCallSearchParameters(), new PaginationParameters());

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
            var paginationParameters = new PaginationParameters { PageSize = pageSize, PageNumber = pageNumber };

            // Act
            var result = _controller.Get(new FundingCallSearchParameters(), paginationParameters);

            // Assert
            result.Should().NotBeNull();
            _mockSearchService.Verify(m => m.Search(It.IsAny<FundingCallSearchParameters>(), pageNumber, maximumPageSize));
        }


        public void Dispose()
        {
        }
    }
}
