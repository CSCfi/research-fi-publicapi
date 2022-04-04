using Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Middleware
{
    public class GlobalErrorHandlerMiddlewareTest
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly Mock<ILogger<GlobalErrorHandlerMiddleware>> _loggerMock;
        private readonly GlobalErrorHandlerMiddleware _middleware;
        private readonly Exception _testException = new("this is a test exception.");

        public GlobalErrorHandlerMiddlewareTest()
        {
            _requestDelegate = async (HttpContext ctx) =>
            {
                await Task.FromException(_testException);
            };

            _loggerMock = new Mock<ILogger<GlobalErrorHandlerMiddleware>>();
            _middleware = new GlobalErrorHandlerMiddleware(_requestDelegate, _loggerMock.Object);

        }

        [Fact]
        public async Task ShouldLogExceptions()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            await _middleware.InvokeAsync(httpContext);

            // Assert
            httpContext.Response.StatusCode.Should().Be(500);

            _loggerMock.Verify(x => x.Log(LogLevel.Error,
               It.IsAny<EventId>(),
               It.IsAny<It.IsAnyType>(),
               _testException,
               (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
               Times.Once);


        }
    }
}
