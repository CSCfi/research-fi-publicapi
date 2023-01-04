using System.Linq;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Middleware;

public class CorrelationIdMiddlewareTest
{
    private const string CorrelationIdHeader = "x-correlation-id";
    private readonly CorrelationIdMiddleware _middleware;

    public CorrelationIdMiddlewareTest()
    {
        async Task RequestDelegate(HttpContext ctx)
        {
            await Task.CompletedTask;
        }

        var loggerMock = new Mock<ILogger<CorrelationIdMiddleware>>();
        _middleware = new CorrelationIdMiddleware(RequestDelegate, loggerMock.Object);

    }

    [Fact]
    public async Task ShouldAddCorrelationIdToContext_WhenDoesNotExist()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        // Act
        await _middleware.InvokeAsync(httpContext);

        // Assert
        var correlationId = httpContext.Items.SingleOrDefault(x => x.Key as string == CorrelationIdHeader);

        correlationId.Should().NotBeNull();
        correlationId.Value.Should().NotBe(default);
    }

    [Fact]
    public async Task ShouldUseExistingCorrelationId_WhenRequestContainsIt()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add(CorrelationIdHeader, "123");

        // Act
        await _middleware.InvokeAsync(httpContext);

        // Assert
        var correlationId = httpContext.Items.SingleOrDefault(x => x.Key as string == CorrelationIdHeader);

        correlationId.Should().NotBeNull();
        correlationId.Value.Should().Be("123");
    }
}