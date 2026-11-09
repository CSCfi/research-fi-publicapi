using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using FluentAssertions;
using Xunit;
using PublicationResponse = ResearchFi.Publication.Publication;
using InfrastructureServiceResponse = ResearchFi.Infrastructure.Service;

namespace CSC.PublicApi.Interface.Tests;

[Collection(ElasticsearchApiCollection.Name)]
[Trait("Category", "ElasticsearchIntegration")]
public sealed class PublicationSearchSystemTest(ElasticsearchApiFixture fixture)
{
    [Fact]
    public async Task GetPublications_CombinesFiltersAndRequiresNamesOnTheSameAuthor()
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync(
            "v1/publications?name=Phase%20Three%20Synthetic&publicationYear=2025&type=A1" +
            "&authorFirstNames=Synthetic&authorLastName=Researcher");

        var publications = await response.GetResponseObject<List<PublicationResponse>>();

        var publication = publications.Should().ContainSingle().Which;
        publication.Id.Should().Be("publication-main");
        publication.Name.Should().Be("Phase Three Synthetic Publication");
        publication.PublicationYear.Should().Be("2025");
        publication.Type!.Code.Should().Be("A1");
        publication.Authors.Should().ContainSingle().Which.Orcid.Should().Be("synthetic-orcid");
        response.Headers.GetValues("x-total").Should().Equal("1");
    }

    [Theory]
    [InlineData("", "publication-main,publication-split-authors,publication-co,publication-older")]
    [InlineData("&showOrganisationPartofCoPublication=true",
        "publication-main,publication-part,publication-split-authors,publication-co,publication-older")]
    [InlineData("&hideCoPublications=true", "publication-main,publication-split-authors,publication-older")]
    [InlineData("&showOrganisationPartofCoPublication=true&hideCoPublications=true",
        "publication-main,publication-part,publication-split-authors,publication-older")]
    public async Task GetPublications_AppliesRelationshipVisibilityAndStableYearOrdering(
        string flags, string expectedIds)
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync("v1/publications?name=Phase%20Three%20Synthetic" + flags);

        var publications = await response.GetResponseObject<List<PublicationResponse>>();

        publications.Select(publication => publication.Id).Should().Equal(expectedIds.Split(','));
        response.Headers.GetValues("x-total").Should().Equal(publications.Count.ToString());
    }

    [Theory]
    [InlineData("doi=10.9999%2Fsynthetic-publication", "publication-main")]
    [InlineData("originalPublicationId=original-publication-main", "publication-main")]
    [InlineData("doi=10.9999%2Fsynthetic", null)]
    [InlineData("originalPublicationId=original-publication", null)]
    [InlineData("name=Not%20Present", null)]
    public async Task GetPublications_MatchesExactIdentifiersAndExcludesNonmatches(
        string query, string? expectedId)
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync("v1/publications?" + query);

        var publications = await response.GetResponseObject<List<PublicationResponse>>();

        if (expectedId is null)
        {
            publications.Should().BeEmpty();
            response.Headers.GetValues("x-total").Should().Equal("0");
        }
        else
        {
            publications.Should().ContainSingle().Which.Id.Should().Be(expectedId);
            response.Headers.GetValues("x-total").Should().Equal("1");
        }
    }

    [Theory]
    [InlineData(1, "publication-main")]
    [InlineData(2, "publication-split-authors")]
    [InlineData(3, "publication-co")]
    [InlineData(4, "publication-older")]
    [InlineData(5, null)]
    public async Task GetPublications_PaginatesFilteredResultsWithAccurateTotals(int page, string? expectedId)
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync(
            $"v1/publications?name=Phase%20Three%20Synthetic&pageNumber={page}&pageSize=1");

        var publications = await response.GetResponseObject<List<PublicationResponse>>();

        publications.Select(publication => publication.Id).Should().Equal(
            expectedId is null ? new string[0] : new[] { expectedId });
        response.Headers.GetValues("x-total").Should().Equal("4");
        response.Headers.GetValues("x-page-count").Should().Equal("4");
        response.Headers.GetValues("x-page-size").Should().Equal("1");
        response.Headers.GetValues("x-current-page").Should().Equal(page.ToString());
    }

    [Fact]
    public async Task GetPublication_ReturnsThePublicContractFromTheIndexedIdNotTheDocumentId()
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync("v1/publications/publication-main");

        var publication = await response.GetResponseObject<PublicationResponse>();

        publication.Id.Should().Be("publication-main");
        publication.OriginalPublicationId.Should().Be("original-publication-main");
        publication.Name.Should().Be("Phase Three Synthetic Publication");
        publication.PublicationYear.Should().Be("2025");
        publication.Doi.Should().Be("10.9999/synthetic-publication");
        publication.Authors.Should().ContainSingle().Which.LastName.Should().Be("Researcher");
    }

    [Fact]
    public async Task GetPublication_ReturnsCopublicationRelationshipsAndOrganizationPartDetail()
    {
        using var client = fixture.Factory.CreateClient();
        using var coResponse = await client.GetAsync("v1/publications/publication-co");
        using var partResponse = await client.GetAsync("v1/publications/publication-part");

        var copublication = await coResponse.GetResponseObject<PublicationResponse>();
        var part = await partResponse.GetResponseObject<PublicationResponse>();

        copublication.OrgPublicationIDs.Should().Equal("publication-part");
        var organizationPart = copublication.OrganizationPartsOfCoPublication.Should().ContainSingle().Which;
        organizationPart.Id.Should().Be("publication-part");
        organizationPart.OriginalPublicationId.Should().Be("original-publication-part");
        organizationPart.Organization!.Id.Should().Be("synthetic-organization");
        part.Id.Should().Be("publication-part");
        part.CoPublicationID.Should().Be("publication-co");
    }

    [Theory]
    [InlineData("publication-missing")]
    [InlineData("publication-ma")]
    public async Task GetPublication_ReturnsNotFoundForMissingOrPartialId(string id)
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync("v1/publications/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

[Collection(ElasticsearchApiCollection.Name)]
[Trait("Category", "ElasticsearchIntegration")]
public sealed class InfrastructureServiceSearchSystemTest(ElasticsearchApiFixture fixture)
{
    [Fact]
    public async Task GetServices_CombinesNestedTextInfrastructureOrganizationAndYearFilters()
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync(
            "v1/infrastructures/services?serviceName=Phase%20Three%20Synthetic" +
            "&serviceDescription=Precision%20imaging&isPartOfInfrastructureURN=urn:synthetic:infra:main" +
            "&isPartOfInfrastructureInfraResponsibleOrganizationId=1234567-8" +
            "&serviceStartsOnYear=2020&serviceEndsByYear=2030");

        var services = await response.GetResponseObject<List<InfrastructureServiceResponse>>();

        var service = services.Should().ContainSingle().Which;
        service.ServiceIdentifier!.LocalIdentifier.Should().Be("service-6001");
        service.ServiceIdentifier.KeyIdentifierURN.Should().Be("urn:synthetic:service:6001");
        service.ServiceIdentifier.OtherPid.Should().ContainSingle().Which.PidContent.Should().Be("service-pid-6001");
        service.ServiceName.Should().ContainSingle().Which.TextContent.Should().Be("Phase Three Synthetic Imaging");
        service.ServiceDescription.Should().ContainSingle().Which.TextContent.Should().Be("Precision imaging");
        service.ServiceStartsOn!.Year.Should().Be(2020);
        service.ServiceEndsOn!.Year.Should().Be(2030);
        service.IsPartOfInfrastructure!.InfraIdentifier!.KeyIdentifierURN.Should().Be("urn:synthetic:infra:main");
        response.Headers.GetValues("x-total").Should().Equal("1");
    }

    [Theory]
    [InlineData("localIdentifier=service-6001", "service-6001")]
    [InlineData("keyIdentifierUrn=urn:synthetic:service:6001", "service-6001")]
    [InlineData("otherPersistentIdentifier=service-pid-6001", "service-6001")]
    [InlineData("localIdentifier=service-600", null)]
    [InlineData("keyIdentifierUrn=urn:synthetic:service:600", null)]
    [InlineData("otherPersistentIdentifier=service-pid-600", null)]
    [InlineData("serviceName=Not%20Present", null)]
    public async Task GetServices_MatchesExactIdentifiersAndExcludesNonmatches(string query, string? expectedId)
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync("v1/infrastructures/services?" + query);

        var services = await response.GetResponseObject<List<InfrastructureServiceResponse>>();

        if (expectedId is null)
        {
            services.Should().BeEmpty();
            response.Headers.GetValues("x-total").Should().Equal("0");
        }
        else
        {
            services.Should().ContainSingle().Which.ServiceIdentifier!.LocalIdentifier.Should().Be(expectedId);
            response.Headers.GetValues("x-total").Should().Equal("1");
        }
    }

    [Theory]
    [InlineData("serviceEndsByYear=2030", "service-6001,service-6002")]
    [InlineData("serviceEndsOnYear=2031", "service-6003")]
    [InlineData("serviceEndsByYear=2029", "")]
    [InlineData("serviceStartsOnYear=2019", "")]
    public async Task GetServices_AppliesExactAndInclusiveYearBoundaries(string query, string expectedIds)
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync("v1/infrastructures/services?" + query);

        var services = await response.GetResponseObject<List<InfrastructureServiceResponse>>();

        services.Select(service => service.ServiceIdentifier!.LocalIdentifier).Should().Equal(
            expectedIds.Length == 0 ? new string[0] : expectedIds.Split(','));
    }

    [Fact]
    public async Task GetServices_PaginatesInExportOrderAndPreservesFilteredTotals()
    {
        using var client = fixture.Factory.CreateClient();
        using var response = await client.GetAsync(
            "v1/infrastructures/services?serviceEndsByYear=2030&pageSize=1&pageNumber=2");

        var services = await response.GetResponseObject<List<InfrastructureServiceResponse>>();

        services.Should().ContainSingle().Which.ServiceIdentifier!.LocalIdentifier.Should().Be("service-6002");
        response.Headers.GetValues("x-total").Should().Equal("2");
        response.Headers.GetValues("x-page-count").Should().Equal("2");
        response.Headers.GetValues("x-current-page").Should().Equal("2");
        response.Headers.GetValues("x-page-size").Should().Equal("1");
    }
}
