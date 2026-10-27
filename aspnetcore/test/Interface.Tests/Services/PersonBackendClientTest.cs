using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Services;
using Microsoft.Extensions.Logging.Abstractions;
using ResearchFi.PersonPublicApi;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Services;

public class PersonBackendClientTest
{
    [Fact]
    public async Task SearchPersonsAsync_WhenBackendReturnsBadRequest_ReturnsErrorStatusCode()
    {
        using var httpClient = new HttpClient(new StubHttpMessageHandler(
            HttpStatusCode.BadRequest,
            "A request body is required.",
            "text/plain"))
        {
            BaseAddress = new Uri("https://person-backend.example/")
        };
        var client = new PersonBackendClient(httpClient, NullLogger<PersonBackendClient>.Instance);

        var (response, errorStatusCode, errorContent, errorContentType) = await client.SearchPersonsAsync(null!, null);

        Assert.Null(response);
        Assert.Equal(HttpStatusCode.BadRequest, errorStatusCode);
        Assert.Equal("A request body is required.", errorContent);
        Assert.Equal("text/plain", errorContentType);
    }

    private sealed class StubHttpMessageHandler(HttpStatusCode statusCode, string content, string contentType) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content)
                {
                    Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType) }
                }
            });
        }
    }
}
