using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Services.V1;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using ResearchFi;
using ResearchFi.CodeList;
using ResearchFi.FundingDecision;
using ResearchFi.Query;
using Xunit;
using ApiFundingDecision = ResearchFi.FundingDecision.FundingDecision;

namespace CSC.PublicApi.Interface.Tests;

public class FundingDecisionHttpContractTest
{
    [Fact]
    public async Task Search_BindsFiltersAndReturnsNestedPublicContractWithPagination()
    {
        var service = new Mock<IFundingDecisionService>(MockBehavior.Strict);
        service.Setup(instance => instance.GetFundingDecisions(
                It.IsAny<GetFundingDecisionQueryParameters>(),
                It.IsAny<PaginationQueryParameters>()))
            .Returns((GetFundingDecisionQueryParameters filters, PaginationQueryParameters pagination) =>
            {
                Assert.Equal("Synthetic decision", filters.Name);
                Assert.Equal("Synthetic funder", filters.FunderName);
                Assert.Equal("1234567-8", filters.FunderId);
                Assert.Equal("PROJECT-42", filters.FunderProjectNumber);
                Assert.Equal(2024, filters.FundingStartYearFrom);
                Assert.Equal("Synthetic organization", filters.FundedOrganization);
                Assert.Equal("9876543-2", filters.FundedOrganizationId);
                Assert.Equal("Ada", filters.FundedPersonFirstName);
                Assert.Equal("Lovelace", filters.FundedPersonLastName);
                Assert.Equal("0000-0001-2345-6789", filters.FundedPersonOrcid);
                Assert.Equal("synthetic-type", filters.TypeOfFunding);
                Assert.Equal(2, pagination.PageNumber);
                Assert.Equal(5, pagination.PageSize);

                return Task.FromResult(((IEnumerable<ApiFundingDecision>)new[]
                {
                    new ApiFundingDecision
                    {
                        NameEn = "Synthetic funding decision",
                        FundingStartDate = new DateTime(2024, 1, 2),
                        AmountInEur = 1250.50m,
                        Funder = new Funder
                        {
                            NameEn = "Synthetic funder",
                            Pids = new List<PersistentIdentifier>
                            {
                                new() { Type = "BUSINESSID", Content = "1234567-8" }
                            }
                        },
                        FundingReceivers = new List<FundingReceiver>
                        {
                            new()
                            {
                                Person = new Person
                                {
                                    FirstNames = "Ada",
                                    LastName = "Lovelace",
                                    OrcId = "0000-0001-2345-6789"
                                },
                                Organization = new Organization
                                {
                                    NameEn = "Synthetic organization",
                                    Pids = new List<PersistentIdentifier>
                                    {
                                        new() { Type = "BUSINESSID", Content = "9876543-2" }
                                    }
                                },
                                FunderProjectNumber = "PROJECT-42",
                                ShareOfFundingInEur = 1250.50m
                            }
                        },
                        TypeOfFunding = new FundingType { Code = "synthetic-type" }
                    }
                }, new SearchResult(pagination.PageNumber, pagination.PageSize, 12)));
            });

        using var factory = Factory();
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFundingDecisionService>();
            services.AddSingleton(service.Object);
        }));
        using var client = host.CreateClient();

        using var response = await client.GetAsync(
            "/v1/funding-decisions?name=Synthetic%20decision&funderName=Synthetic%20funder" +
            "&funderId=1234567-8&funderProjectNumber=PROJECT-42&fundingStartYearFrom=2024" +
            "&fundedOrganization=Synthetic%20organization&fundedOrganizationId=9876543-2" +
            "&fundedPersonFirstName=Ada&fundedPersonLastName=Lovelace" +
            "&fundedPersonOrcid=0000-0001-2345-6789&typeOfFunding=synthetic-type&pageNumber=2&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("2", Assert.Single(response.Headers.GetValues("x-current-page")));
        Assert.Equal("5", Assert.Single(response.Headers.GetValues("x-page-size")));
        Assert.Equal("12", Assert.Single(response.Headers.GetValues("x-total")));
        using var actual = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        using var expected = JsonDocument.Parse(
            """[{"nameEn":"Synthetic funding decision","fundingStartDate":"2024-01-02T00:00:00","amountInEur":1250.50,"funder":{"nameEn":"Synthetic funder","pids":[{"type":"BUSINESSID","content":"1234567-8"}]},"fundingReceivers":[{"person":{"firstNames":"Ada","lastName":"Lovelace","orcId":"0000-0001-2345-6789","fundedPerson":false},"organization":{"nameEn":"Synthetic organization","pids":[{"type":"BUSINESSID","content":"9876543-2"}],"isFinnishOrganization":false},"funderProjectNumber":"PROJECT-42","shareOfFundingInEur":1250.50}],"typeOfFunding":{"code":"synthetic-type"}}]""");
        Assert.True(JsonElement.DeepEquals(expected.RootElement, actual.RootElement), actual.RootElement.ToString());
        service.Verify(instance => instance.GetFundingDecisions(
            It.IsAny<GetFundingDecisionQueryParameters>(),
            It.IsAny<PaginationQueryParameters>()), Times.Once);
        service.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("fundingStartYearFrom=not-a-year")]
    [InlineData("pageNumber=broken")]
    [InlineData("pageSize=broken")]
    public async Task Search_InvalidQueryReturns400BeforeServiceCall(string query)
    {
        var service = new Mock<IFundingDecisionService>(MockBehavior.Strict);
        using var factory = Factory();
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFundingDecisionService>();
            services.AddSingleton(service.Object);
        }));
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/v1/funding-decisions?" + query);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        service.VerifyNoOtherCalls();
    }

    private static TestWebApplicationFactory<Program> Factory() => new()
    {
        AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
        Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.FundingDecision.Read] }
    };
}
