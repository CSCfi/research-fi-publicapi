using Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Middleware
{
    public class CorrelationIdMiddlewareTest
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly CorrelationIdMiddleware _middleware;

        public CorrelationIdMiddlewareTest()
        {
            _requestDelegate = async (HttpContext ctx) =>
            {
                await Task.CompletedTask;
            };
            _middleware = new CorrelationIdMiddleware(_requestDelegate);

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
}
