using CSC.PublicApi.Interface.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CSC.PublicApi.Tests.Middleware;

public class CorrelationIdMiddlewareTest
{
    private readonly RequestDelegate _requestDelegate;
    private readonly CorrelationIdMiddleware _middleware;

    public CorrelationIdMiddlewareTest()
    {
        _requestDelegate = async (ctx) =>
        {
            await Task.CompletedTask;
        };

        var loggerMock = new Mock<ILogger<CorrelationIdMiddleware>>();
        _middleware = new CorrelationIdMiddleware(_requestDelegate, loggerMock.Object);

    }

    [Fact]
    public async Task ShouldAddCorrelationIdToContext_WhenDoesNotExist()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        // Act
        await _middleware.InvokeAsync(httpContext);

        // Assert
        var correlationId = httpContext.Items.SingleOrDefault(x => x.Key as string == "X-Correlation-ID");

        correlationId.Should().NotBeNull();
        correlationId.Value.Should().NotBe(default);
    }

    [Fact]
    public async Task ShouldUseExistingCorrelationId_WhenRequestContainsIt()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("X-Correlation-ID", "123");

        // Act
        await _middleware.InvokeAsync(httpContext);

        // Assert
        var correlationId = httpContext.Items.SingleOrDefault(x => x.Key as string == "X-Correlation-ID");

        correlationId.Should().NotBeNull();
        correlationId.Value.Should().Be("123");
    }
}