using System.Linq;
using System;
using System.Net.Http;
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

    [Fact]
    public async Task InvokeAsync_AddsExistingCorrelationIdToResponseHeader()
    {
        using var server = CreateServer();
        using var client = server.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Add(CorrelationIdHeader, "request-id");

        using var response = await client.SendAsync(request);

        response.Headers.GetValues(CorrelationIdHeader).Should().ContainSingle().Which.Should().Be("request-id");
        (await response.Content.ReadAsStringAsync()).Should().Be("ok");
    }

    [Fact]
    public async Task InvokeAsync_GeneratesGuidCorrelationIdAndMakesItAvailableDownstream()
    {
        using var server = CreateServer();
        using var client = server.CreateClient();

        using var response = await client.GetAsync("/");

        var responseId = response.Headers.GetValues(CorrelationIdHeader).Should().ContainSingle().Which;
        Guid.TryParse(responseId, out _).Should().BeTrue();
        (await response.Content.ReadAsStringAsync()).Should().Be("ok");
    }

    private static TestServer CreateServer() => new(new WebHostBuilder().Configure(app =>
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.Run(context => context.Response.WriteAsync("ok"));
    }));
}