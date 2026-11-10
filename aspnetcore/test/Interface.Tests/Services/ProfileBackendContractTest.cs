using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Services;
using Microsoft.Extensions.Logging.Abstractions;
using ResearchFi.ProfilePublicApi;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Services;

public class ProfileBackendContractTest
{
    internal const string SuccessJson =
        """{"profileFound":true,"controllerFound":true,"consentActive":false,"consentManagementURL":{"en":"https://consent.test/synthetic"},"profileData":null}""";

    [Theory]
    [InlineData(null)]
    [InlineData("synthetic-client")]
    public async Task SearchProfiles_PostsContractAndOptionalClientIdentity(string? clientId)
    {
        var request = new ProfileDataRequest { ControllerROR = "https://ror.test/synthetic-controller" };
        var calls = 0;
        using var httpClient = CreateClient(new Handler(async (message, cancellationToken) =>
        {
            calls++;
            Assert.Equal(HttpMethod.Post, message.Method);
            Assert.Equal("https://profile-backend.test/api/publicapi/profile", message.RequestUri!.AbsoluteUri);
            Assert.Equal("application/json", message.Content!.Headers.ContentType!.MediaType);
            using var actual = JsonDocument.Parse(await message.Content.ReadAsStringAsync(cancellationToken));
            using var expected = JsonDocument.Parse(JsonSerializer.Serialize(request, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
            Assert.True(JsonElement.DeepEquals(expected.RootElement, actual.RootElement));
            Assert.Equal(clientId != null, message.Headers.Contains("public-api-clientid"));
            if (clientId != null)
                Assert.Equal(new[] { clientId }, message.Headers.GetValues("public-api-clientid"));
            return Response(HttpStatusCode.OK, SuccessJson, "application/json");
        }));
        var client = new ProfileBackendClient(httpClient, NullLogger<ProfileBackendClient>.Instance);

        var (response, status, content, contentType) = await client.SearchProfilesAsync(request, clientId);

        Assert.NotNull(response);
        Assert.True(response!.ProfileFound);
        Assert.True(response.ControllerFound);
        Assert.False(response.ConsentActive);
        Assert.Equal("https://consent.test/synthetic", response.ConsentManagementURL!.En);
        Assert.Null(response.ProfileData);
        Assert.Null(status);
        Assert.Null(content);
        Assert.Null(contentType);
        Assert.Equal(1, calls);
    }

    [Theory]
    [InlineData(400, "application/problem+json", """{"title":"Synthetic invalid request"}""")]
    [InlineData(401, "text/plain", "Synthetic unauthorized")]
    [InlineData(403, "text/plain", "Synthetic forbidden")]
    [InlineData(429, "application/json", """{"error":"Synthetic rate limit"}""")]
    [InlineData(500, "text/plain", "Synthetic backend error")]
    [InlineData(503, "text/plain", "")]
    public async Task SearchProfiles_PreservesBackendError(int code, string mediaType, string body)
    {
        using var httpClient = CreateClient(new Handler((_, _) =>
            Task.FromResult(Response((HttpStatusCode)code, body, mediaType))));
        var client = new ProfileBackendClient(httpClient, NullLogger<ProfileBackendClient>.Instance);

        var (response, status, content, contentType) = await client.SearchProfilesAsync(new ProfileDataRequest(), null);

        Assert.Null(response);
        Assert.Equal((HttpStatusCode)code, status);
        Assert.Equal(body, content);
        Assert.Equal($"{mediaType}; charset=utf-8", contentType);
    }

    [Theory]
    [InlineData("")]
    [InlineData("{broken")]
    [InlineData("[]")]
    public async Task SearchProfiles_InvalidSuccessJsonThrows(string body)
    {
        using var httpClient = CreateClient(new Handler((_, _) =>
            Task.FromResult(Response(HttpStatusCode.OK, body, "application/json"))));
        var client = new ProfileBackendClient(httpClient, NullLogger<ProfileBackendClient>.Instance);

        await Assert.ThrowsAsync<JsonException>(() => client.SearchProfilesAsync(new ProfileDataRequest(), null));
    }

    [Fact]
    public async Task SearchProfiles_JsonNullCurrentlyReturnsNoResponseAndNoError()
    {
        // P4-PROFILE: a null success payload is not a verified profile response.
        using var httpClient = CreateClient(new Handler((_, _) =>
            Task.FromResult(Response(HttpStatusCode.OK, "null", "application/json"))));
        var client = new ProfileBackendClient(httpClient, NullLogger<ProfileBackendClient>.Instance);

        var result = await client.SearchProfilesAsync(new ProfileDataRequest(), null);

        Assert.Null(result.Response);
        Assert.Null(result.ErrorStatusCode);
        Assert.Null(result.ErrorContent);
        Assert.Null(result.ErrorContentType);
    }

    [Fact]
    public async Task SearchProfiles_NetworkExceptionPropagates()
    {
        var error = new HttpRequestException("Synthetic connection failure");
        using var httpClient = CreateClient(new Handler((_, _) => Task.FromException<HttpResponseMessage>(error)));
        var client = new ProfileBackendClient(httpClient, NullLogger<ProfileBackendClient>.Instance);

        var thrown = await Assert.ThrowsAsync<HttpRequestException>(() =>
            client.SearchProfilesAsync(new ProfileDataRequest(), null));

        Assert.Same(error, thrown);
    }

    [Fact]
    public async Task SearchProfiles_CancellationInterruptsInFlightRequest()
    {
        using var cancellation = new CancellationTokenSource();
        var started = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var httpClient = CreateClient(new Handler(async (_, token) =>
        {
            Assert.True(token.CanBeCanceled);
            started.SetResult(true);
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            throw new InvalidOperationException("The canceled request must not complete normally.");
        }));
        var client = new ProfileBackendClient(httpClient, NullLogger<ProfileBackendClient>.Instance);
        var pending = client.SearchProfilesAsync(new ProfileDataRequest(), null, cancellation.Token);
        await started.Task.WaitAsync(TimeSpan.FromSeconds(10));

        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => pending.WaitAsync(TimeSpan.FromSeconds(10)));
    }

    internal static HttpResponseMessage Response(HttpStatusCode status, string body, string mediaType) => new(status)
    {
        Content = new StringContent(body, Encoding.UTF8, mediaType)
    };

    private static HttpClient CreateClient(HttpMessageHandler handler) => new(handler)
    {
        BaseAddress = new Uri("https://profile-backend.test/")
    };

    internal sealed class Handler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            send(request, cancellationToken);
    }
}
