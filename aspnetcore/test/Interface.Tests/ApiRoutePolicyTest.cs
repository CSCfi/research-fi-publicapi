using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CSC.PublicApi.ElasticService;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using ImportLink = CSC.PublicApi.Interface.Models.ImportDb.Entities.ImpLinkGrantedFundingToPublication;

namespace CSC.PublicApi.Interface.Tests;

public class ApiRoutePolicyTest
{
    public static IEnumerable<object[]> Routes()
    {
        var routes = new (string Method, string Path, string Policy, int Status)[]
        {
            ("GET", "/v1/funding-calls", ApiPolicies.FundingCall.Read, 200),
            ("GET", "/v1/funding-calls-export", ApiPolicies.FundingCall.Read, 200),
            ("GET", "/v1/funding-decisions", ApiPolicies.FundingDecision.Read, 200),
            ("GET", "/v1/funding-decisions-export", ApiPolicies.FundingDecision.Read, 200),
            ("GET", "/v1/publications", ApiPolicies.Publication.Read, 200),
            ("GET", "/v1/publications-export", ApiPolicies.Publication.Read, 200),
            ("GET", "/v1/publications/missing-synthetic", ApiPolicies.Publication.Read, 404),
            ("GET", "/v1/research-datasets", ApiPolicies.ResearchDataset.Read, 200),
            ("GET", "/v2/research-datasets", ApiPolicies.ResearchDataset.Read, 200),
            ("GET", "/v1/research-datasets-export", ApiPolicies.ResearchDataset.Read, 200),
            ("GET", "/v1/organizations", ApiPolicies.Organization.Read, 200),
            ("GET", "/v1/infrastructures", ApiPolicies.Infrastructure.Read, 200),
            ("GET", "/v1/infrastructures/services", ApiPolicies.Infrastructure.Read, 200),
            ("GET", "/v1/infrastructures-export", ApiPolicies.Infrastructure.Read, 200),
            ("GET", "/v1/infrastructures-export/services", ApiPolicies.Infrastructure.Read, 200),
            ("GET", "/VirtaJulkaisutietopalvelu/Latausraportit/duplikaatit", ApiPolicies.Publication.Read, 200),
            ("GET", "/VirtaJulkaisutietopalvelu/Latausraportit/tilanne", ApiPolicies.Publication.Read, 200),
            ("GET", "/VirtaJulkaisutietopalvelu/Latausraportit/virheet", ApiPolicies.Publication.Read, 200),
            ("GET", "/VirtaJulkaisutietopalvelu/Yhteisjulkaisut/ristiriitaiset", ApiPolicies.Publication.Read, 200),
            ("GET", "/v1/funders/synthetic-grant/publications/synthetic-org", ApiPolicies.Funder.Read, 200),
            ("GET", "/v1/funders/synthetic-grant/publications/synthetic-org/missing", ApiPolicies.Funder.Read, 404),
            ("POST", "/v1/funders/synthetic-grant/publications/synthetic-org", ApiPolicies.Funder.Write, 201),
            ("PUT", "/v1/funders/synthetic-grant/publications/synthetic-org/missing", ApiPolicies.Funder.Write, 404),
            ("DELETE", "/v1/funders/synthetic-grant/publications/synthetic-org/missing", ApiPolicies.Funder.Write, 404)
        };
        foreach (var route in routes)
        foreach (var mode in new[] { "anonymous", "no-role", "wrong-role", "correct-role" })
            yield return new object[] { route.Method, route.Path, route.Policy, route.Status, mode };
    }

    [Theory]
    [MemberData(nameof(Routes))]
    public async Task Route_EnforcesItsOwnPolicyWithIsolatedDependencies(
        string method, string path, string policy, int authorizedStatus, string mode)
    {
        using var factory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = mode == "anonymous" ? TestAuthenticationMode.Anonymous :
                mode == "no-role" ? TestAuthenticationMode.AuthenticatedWithoutRoles :
                TestAuthenticationMode.AuthenticatedWithRoles,
            Roles = mode == "correct-role" ? new[] { ApiPolicies.PolicyRoleMap[policy] } :
                new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.Profile.Read] },
            Claims = new Dictionary<string, string> { ["clientId"] = "synthetic-client" }
        };
        var searchCalls = new List<string>();
        var database = $"route-{Guid.NewGuid():N}";
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(ISearchService<,>));
            services.AddSingleton(searchCalls);
            services.AddScoped(typeof(ISearchService<,>), typeof(RecordingEmptySearchService<,>));
            services.RemoveAll<ImportDbContext>();
            services.AddScoped(_ => new ImportDbContext(new DbContextOptionsBuilder<ImportDbContext>()
                .UseInMemoryDatabase(database + "-import").Options));
            services.RemoveAll<VirtaJtpDbContext>();
            services.AddScoped(_ => new VirtaJtpDbContext(new DbContextOptionsBuilder<VirtaJtpDbContext>()
                .UseInMemoryDatabase(database + "-virta").Options));
        }));
        using var client = host.CreateClient();
        using var request = new HttpRequestMessage(new HttpMethod(method), path);
        if (method is "POST" or "PUT")
            request.Content = new StringContent(
                """{"publicationId":"synthetic-publication","grantedFundingId":"synthetic-grant","organizationId":"synthetic-org"}""",
                Encoding.UTF8, "application/json");

        using var response = await client.SendAsync(request);

        var expected = mode == "anonymous" ? 401 : mode == "correct-role" ? authorizedStatus : 403;
        Assert.Equal((HttpStatusCode)expected, response.StatusCode);
        if (mode != "correct-role")
            Assert.Empty(searchCalls);
        else if (method == "GET" && authorizedStatus == 200)
        {
            Assert.Equal("[]", await response.Content.ReadAsStringAsync());
            if (!path.Contains("/funders/", StringComparison.Ordinal) &&
                !path.StartsWith("/Virta", StringComparison.Ordinal))
                Assert.Single(searchCalls);
        }
        if (method == "POST" && mode == "correct-role")
        {
            Assert.NotNull(response.Headers.Location);
            using var fetched = await client.GetAsync(response.Headers.Location);
            // A write role alone must not grant access to the read endpoint.
            Assert.Equal(HttpStatusCode.Forbidden, fetched.StatusCode);
            using var scope = host.Services.CreateScope();
            var stored = await scope.ServiceProvider.GetRequiredService<ImportDbContext>()
                .ImpLinkGrantedFundingToPublications.SingleAsync();
            Assert.Equal("synthetic-client", stored.ClientId);
            Assert.Equal("synthetic-publication", stored.PublicationId);
        }
    }

    [Fact]
    public async Task FunderPut_UpdatesMatchingLinkForAuthenticatedClient()
    {
        const string database = "funder-put-" + nameof(FunderPut_UpdatesMatchingLinkForAuthenticatedClient);
        var created = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        using var factory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
            Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.Funder.Write] },
            Claims = new Dictionary<string, string> { ["clientId"] = "synthetic-client" }
        };
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ImportDbContext>();
            services.AddScoped(_ => new ImportDbContext(new DbContextOptionsBuilder<ImportDbContext>()
                .UseInMemoryDatabase(database).Options));
        }));
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ImportDbContext>();
            context.ImpLinkGrantedFundingToPublications.AddRange(
                new ImportLink
                {
                    Id = 1,
                    ClientId = "synthetic-client",
                    OrganizationId = "old-org",
                    FunderProjectNumber = "old-grant",
                    PublicationId = "old-publication",
                    Doi = "10.synthetic/old",
                    Created = created
                },
                new ImportLink
                {
                    Id = 2,
                    ClientId = "other-client",
                    OrganizationId = "old-org",
                    FunderProjectNumber = "old-grant",
                    PublicationId = "old-publication",
                    Doi = "10.synthetic/old",
                    Created = created
                });
            await context.SaveChangesAsync();
        }

        using var client = host.CreateClient();
        using var response = await client.PutAsync(
            "/v1/funders/old-grant/publications/old-org/old-publication",
            new StringContent(
                """{"grantedFundingId":"new-grant","organizationId":"new-org","publicationId":"new-publication","doi":"10.synthetic/new"}""",
                Encoding.UTF8,
                "application/json"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var verifyScope = host.Services.CreateScope();
        var verifiedContext = verifyScope.ServiceProvider.GetRequiredService<ImportDbContext>();
        var updated = await verifiedContext.ImpLinkGrantedFundingToPublications.SingleAsync(link => link.Id == 1);
        Assert.Equal("new-grant", updated.FunderProjectNumber);
        Assert.Equal("new-org", updated.OrganizationId);
        Assert.Equal("new-publication", updated.PublicationId);
        Assert.Equal("10.synthetic/new", updated.Doi);
        Assert.Equal(created, updated.Created);
        Assert.True(updated.Modified > created);
        var otherClientLink = await verifiedContext.ImpLinkGrantedFundingToPublications
            .SingleAsync(link => link.Id == 2);
        Assert.Equal("old-grant", otherClientLink.FunderProjectNumber);
        Assert.Equal("old-publication", otherClientLink.PublicationId);
    }

    [Fact]
    public async Task FunderDelete_RemovesOnlyMatchingClientLink()
    {
        const string database = "funder-delete-" + nameof(FunderDelete_RemovesOnlyMatchingClientLink);
        using var factory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
            Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.Funder.Write] },
            Claims = new Dictionary<string, string> { ["clientId"] = "synthetic-client" }
        };
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ImportDbContext>();
            services.AddScoped(_ => new ImportDbContext(new DbContextOptionsBuilder<ImportDbContext>()
                .UseInMemoryDatabase(database).Options));
        }));
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ImportDbContext>();
            context.ImpLinkGrantedFundingToPublications.AddRange(
                new ImportLink
                {
                    Id = 1,
                    ClientId = "synthetic-client",
                    OrganizationId = "synthetic-org",
                    FunderProjectNumber = "synthetic-grant",
                    PublicationId = "synthetic-publication",
                    Created = DateTime.UtcNow
                },
                new ImportLink
                {
                    Id = 2,
                    ClientId = "other-client",
                    OrganizationId = "synthetic-org",
                    FunderProjectNumber = "synthetic-grant",
                    PublicationId = "synthetic-publication",
                    Created = DateTime.UtcNow
                });
            await context.SaveChangesAsync();
        }

        using var client = host.CreateClient();
        using var response = await client.DeleteAsync(
            "/v1/funders/synthetic-grant/publications/synthetic-org/synthetic-publication");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using var verifyScope = host.Services.CreateScope();
        var remaining = await verifyScope.ServiceProvider.GetRequiredService<ImportDbContext>()
            .ImpLinkGrantedFundingToPublications.ToListAsync();
        var remainingLink = Assert.Single(remaining);
        Assert.Equal(2, remainingLink.Id);
        Assert.Equal("other-client", remainingLink.ClientId);
    }

    public sealed class RecordingEmptySearchService<TIn, TOut>(List<string> calls) : ISearchService<TIn, TOut>
        where TOut : class
    {
        public Task<(IEnumerable<TOut>, SearchResult)> Search(TIn parameters, int pageNumber, int pageSize)
        {
            calls.Add("search");
            return Task.FromResult(((IEnumerable<TOut>)Array.Empty<TOut>(), new SearchResult(pageNumber, pageSize, 0)));
        }

        public Task<(IEnumerable<TOut>, SearchAfterResult)> SearchAfter(TIn parameters, int pageSize, long? searchAfter)
        {
            calls.Add("export");
            return Task.FromResult(((IEnumerable<TOut>)Array.Empty<TOut>(), new SearchAfterResult(null, pageSize, 0)));
        }

        public Task<TOut?> GetSingle(string id)
        {
            calls.Add("single");
            return Task.FromResult<TOut?>(null);
        }
    }
}
