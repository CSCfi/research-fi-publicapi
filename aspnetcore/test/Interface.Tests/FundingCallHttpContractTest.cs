using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;
using FundingCall = CSC.PublicApi.Service.Models.FundingCall.FundingCall;

namespace CSC.PublicApi.Interface.Tests;

public class FundingCallHttpContractTest
{
    [Fact]
    public async Task Export_FollowsReturnedLinksWithoutDuplicatesAndPreservesFilters()
    {
        var cursors = new List<long?>();
        var search = new Mock<ISearchService<FundingCallSearchParameters, FundingCall>>(MockBehavior.Strict);
        search.Setup(service => service.SearchAfter(It.IsAny<FundingCallSearchParameters>(), 2, It.IsAny<long?>()))
            .Returns((FundingCallSearchParameters parameters, int size, long? cursor) =>
            {
                Assert.Equal("Synthetic & science", parameters.Name);
                Assert.Equal("A1", parameters.Category);
                cursors.Add(cursor);
                var ids = cursor switch
                {
                    null => new[] { 10, 20 },
                    20 => new[] { 30 },
                    30 => Array.Empty<int>(),
                    _ => throw new InvalidOperationException($"Unexpected cursor {cursor}")
                };
                return Task.FromResult(((IEnumerable<FundingCall>)ids.Select(id => new FundingCall
                {
                    NameFi = $"Synthetic {id}", ExportSortId = id
                }).ToArray(), new SearchAfterResult(ids.Length == 0 ? null : ids.Last(), size, 3)));
            });
        using var factory = Factory();
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISearchService<FundingCallSearchParameters, FundingCall>>();
            services.AddSingleton(search.Object);
        }));
        using var client = host.CreateClient();
        var names = new List<string>();
        string? url = "/v1/funding-calls-export?name=Synthetic%20%26%20science&category=A1&pageSize=2";
        var pages = 0;
        while (url != null)
        {
            Assert.True(++pages <= 3, "Export traversal did not terminate.");
            using var response = await client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("2", Assert.Single(response.Headers.GetValues("x-page-size")));
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            names.AddRange(json.RootElement.EnumerateArray().Select(item => item.GetProperty("nameFi").GetString()!));
            Assert.All(json.RootElement.EnumerateArray(), item =>
                Assert.False(item.TryGetProperty("exportSortId", out _)));
            if (response.Headers.TryGetValues("link", out var values))
            {
                var link = Assert.Single(values);
                Assert.EndsWith("; rel=\"next\"", link);
                url = link[1..link.IndexOf('>')];
                Assert.Equal("3", Assert.Single(response.Headers.GetValues("x-total")));
                Assert.True(response.Headers.Contains("x-next-page-token"));
            }
            else
            {
                Assert.Empty(json.RootElement.EnumerateArray());
                Assert.False(response.Headers.Contains("x-next-page-token"));
                url = null;
            }
        }
        Assert.Equal(new[] { "Synthetic 10", "Synthetic 20", "Synthetic 30" }, names);
        Assert.Equal(names.Count, names.Distinct().Count());
        Assert.Equal(new long?[] { null, 20, 30 }, cursors);
        search.Verify(service => service.SearchAfter(It.IsAny<FundingCallSearchParameters>(), 2,
            It.IsAny<long?>()), Times.Exactly(3));
        search.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("pageNumber=broken")]
    [InlineData("pageSize=broken")]
    [InlineData("dateFrom=not-a-date")]
    public async Task Search_InvalidQueryReturns400BeforeDependencyCall(string query)
    {
        var search = new Mock<ISearchService<FundingCallSearchParameters, FundingCall>>(MockBehavior.Strict);
        using var factory = Factory();
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISearchService<FundingCallSearchParameters, FundingCall>>();
            services.AddSingleton(search.Object);
        }));
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/v1/funding-calls?" + query);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        search.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(150, 100)]
    public async Task Search_BindsFiltersAndNormalizedPaginationWithPublicJson(int requestedSize, int expectedSize)
    {
        var search = new Mock<ISearchService<FundingCallSearchParameters, FundingCall>>(MockBehavior.Strict);
        search.Setup(service => service.Search(It.IsAny<FundingCallSearchParameters>(), 2, expectedSize))
            .Returns((FundingCallSearchParameters parameters, int page, int size) =>
            {
                Assert.Equal("Synthetic science", parameters.Name);
                Assert.Equal("Synthetic funder", parameters.FoundationName);
                Assert.Equal("1234567-8", parameters.FoundationBusinessId);
                Assert.Equal("A1", parameters.Category);
                Assert.Equal(new DateTime(2025, 1, 2), parameters.DateFrom);
                Assert.Equal(new DateTime(2025, 12, 30), parameters.DateTo);
                return Task.FromResult(((IEnumerable<FundingCall>)new[]
                {
                    new FundingCall { NameFi = "Synthetic call", ContinuousApplication = true, ExportSortId = 999 }
                }, new SearchResult(page, size, 201)));
            });
        using var factory = Factory();
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISearchService<FundingCallSearchParameters, FundingCall>>();
            services.AddSingleton(search.Object);
        }));
        using var client = host.CreateClient();

        using var response = await client.GetAsync(
            "/v1/funding-calls?name=Synthetic%20science&foundationName=Synthetic%20funder" +
            "&foundationBusinessId=1234567-8&category=A1&dateFrom=2025-01-02&dateTo=2025-12-30" +
            $"&pageNumber=2&pageSize={requestedSize}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedSize.ToString(), Assert.Single(response.Headers.GetValues("x-page-size")));
        Assert.Equal("2", Assert.Single(response.Headers.GetValues("x-current-page")));
        Assert.Equal("201", Assert.Single(response.Headers.GetValues("x-total")));
        using var actual = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        using var expected = JsonDocument.Parse("""[{"nameFi":"Synthetic call","continuousApplication":true}]""");
        Assert.True(JsonElement.DeepEquals(expected.RootElement, actual.RootElement), actual.RootElement.ToString());
        search.Verify(service => service.Search(It.IsAny<FundingCallSearchParameters>(), 2, expectedSize), Times.Once);
        search.VerifyNoOtherCalls();
    }

    private static TestWebApplicationFactory<Program> Factory() => new()
    {
        AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
        Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.FundingCall.Read] }
    };
}
