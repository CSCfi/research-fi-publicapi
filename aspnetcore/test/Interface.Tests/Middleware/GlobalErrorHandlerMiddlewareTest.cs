using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Middleware;

public class GlobalErrorHandlerMiddlewareTest
{
    private readonly RequestDelegate _requestDelegate;
    private readonly Mock<ILogger<GlobalErrorHandlerMiddleware>> _loggerMock;
    private readonly GlobalErrorHandlerMiddleware _middleware;
    private readonly Exception _testException = new("this is a test exception.");

    public GlobalErrorHandlerMiddlewareTest()
    {
        _requestDelegate = async (ctx) =>
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

    [Fact]
    public async Task InvokeAsync_ReturnsSanitizedJsonWithCorrelationId()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Items[CorrelationIdMiddleware.CorrelationIdHeaderName] = "trace-123";
        var middleware = new GlobalErrorHandlerMiddleware(
            _ => throw new InvalidOperationException("private exception detail"),
            _loggerMock.Object);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(500);
        context.Response.ContentType.Should().Be("application/json");
        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        document.RootElement.GetProperty("message").GetString().Should().Be("Error. Correlation Id: trace-123");
        document.RootElement.ToString().Should().NotContain("private exception detail");
    }

    [Fact]
    public async Task InvokeAsync_UsesFallbackWhenCorrelationIdIsUnavailable()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new GlobalErrorHandlerMiddleware(
            _ => throw new InvalidOperationException("private exception detail"),
            _loggerMock.Object);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(500);
        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        document.RootElement.GetProperty("message").GetString().Should().Be("Error. Correlation Id: N/A");
        document.RootElement.ToString().Should().NotContain("private exception detail");
    }

    [Fact]
    public async Task CorrelationAndErrorMiddleware_ReturnMatchingHeaderAndSanitizedBody()
    {
        using var server = new TestServer(new WebHostBuilder().Configure(app =>
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            app.Run(_ => Task.FromException(_testException));
        }));
        using var client = server.CreateClient();

        using var response = await client.GetAsync("/");
        var responseCorrelationId = response.Headers.GetValues(CorrelationIdMiddleware.CorrelationIdHeaderName)
            .Should().ContainSingle().Which;
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var message = document.RootElement.GetProperty("message").GetString();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        message.Should().Be($"Error. Correlation Id: {responseCorrelationId}");
        message.Should().NotContain(_testException.Message);
    }
}