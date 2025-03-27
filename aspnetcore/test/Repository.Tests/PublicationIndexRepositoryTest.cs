using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Publication;
using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Organization = CSC.PublicApi.Service.Models.Organization.Organization;
namespace Repository.Tests;

public class PublicationIndexRepositoryTest
{
    private readonly PublicationIndexRepository _publicationIndexRepository;
    
    /// <summary>
    /// Unit test for verifying the behavior of the <see cref="PublicationIndexRepository.PerformInMemoryOperations"/> method.
    /// </summary>
    [Fact]
    public void PerformInMemoryOperations_Publication_IsHandledCorrectly()
    {
        // Arrange
        var publications = new List<object>
        {
            GetModel()
        };
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var organization1 = new Organization
        {
            Id = 123,
            OrganizationId = "organizationId 1",
            NameFi = "Main organization 1",
        };
        var organization2 = new Organization
        {
            Id = 234,
            OrganizationId = "organizationId 2",
            NameFi = "Main organization 2",
        };
        var organization3 = new Organization
        {
            Id = 345,
            ParentId = 234,
            LocalOrganizationUnitId = "localOrganizationUnitId 999",
            NameFi = "Unit of organization 2",
        };
        memoryCache.Set(MemoryCacheKeys.OrganizationById(123), organization1);
        memoryCache.Set(MemoryCacheKeys.OrganizationById(234), organization2);
        memoryCache.Set(MemoryCacheKeys.OrganizationById(345), organization3);

        // Initialize repository with memory cache
        var _publicationIndexRepository = new PublicationIndexRepository(null, null, memoryCache);

        // Act
        var resultPublications = _publicationIndexRepository.PerformInMemoryOperations(publications);
        
        // Assert
        resultPublications.Should().NotBeNullOrEmpty();
        var publicationObject = publications.FirstOrDefault();
        publicationObject.Should().NotBeNull().And.BeAssignableTo<Publication>();
        var publication = (Publication)publicationObject!;

        // Check that PeerReviewed is set correctly in HandlePeerReviewed
        publication.PeerReviewed.Code.Should().Be("1");
        publication.PeerReviewed.NameFi.Should().Be("Vertaisarvioitu");
        publication.PeerReviewed.NameSv.Should().Be("Kollegialt utvärderad");
        publication.PeerReviewed.NameEn.Should().Be("Peer-Reviewed");

        // Check that ParentPublication is set correctly in HandleParentPublications
        publication.ParentPublication.Should().NotBeNull();
        publication.ParentPublication.Name.Should().Be("Parent publication name");
        publication.ParentPublication.Publisher.Should().Be("Parent publication publisher");
        publication.ParentPublication.Type.Should().NotBeNull();
        publication.ParentPublication.Type.Code.Should().Be("code publication type");
        publication.ParentPublication.Type.NameFi.Should().Be("name publication type name FI");
        publication.ParentPublication.Type.NameSv.Should().Be("name publication type name SV");
        publication.ParentPublication.Type.NameEn.Should().Be("name publication type name EN");

        // Check that Authors are set correctly in HandleAuthors
        publication.Authors.Should().NotBeNullOrEmpty();
        publication.Authors[0].FirstNames.Should().Be("Author 1 first name");
        publication.Authors[0].LastName.Should().Be("Author 1 last name");
        publication.Authors[0].Orcid.Should().Be("Author 1 ORCID ID");
        publication.Authors[0].Organizations[0].Should().NotBeNull();
        publication.Authors[0].Organizations[0].Id.Should().Be("organizationId 1");
        publication.Authors[0].Organizations[0].UnitIds.Should().BeNullOrEmpty();
        publication.Authors[0].ArtPublicationRole.Should().NotBeNull();
        publication.Authors[0].ArtPublicationRole.Code.Should().Be("code 1");
        publication.Authors[0].ArtPublicationRole.NameFi.Should().Be("name 1 FI");
        publication.Authors[0].ArtPublicationRole.NameSv.Should().Be("name 1 SV");
        publication.Authors[0].ArtPublicationRole.NameEn.Should().Be("name 1 EN");
        publication.Authors[1].FirstNames.Should().Be("Author 2 first name");
        publication.Authors[1].LastName.Should().Be("Author 2 last name");
        publication.Authors[1].Orcid.Should().Be("Author 2 ORCID ID");
        publication.Authors[1].Organizations[0].Should().NotBeNull();
        publication.Authors[1].Organizations[0].Id.Should().Be("organizationId 2");
        publication.Authors[1].Organizations[0].UnitIds.Should().NotBeNullOrEmpty();
        publication.Authors[1].Organizations[0].UnitIds[0].Should().Be("localOrganizationUnitId 999");
        publication.Authors[1].ArtPublicationRole.Should().NotBeNull();
        publication.Authors[1].ArtPublicationRole.Code.Should().Be("code 2");
        publication.Authors[1].ArtPublicationRole.NameFi.Should().Be("name 2 FI");
        publication.Authors[1].ArtPublicationRole.NameSv.Should().Be("name 2 SV");
        publication.Authors[1].ArtPublicationRole.NameEn.Should().Be("name 2 EN");

        // Check that Organizations are set correctly in HandleOrganizations
        publication.Organizations.Should().NotBeNullOrEmpty();
        publication.Organizations[0].Id.Should().Be("organizationId 1");
        publication.Organizations[0].NameFi.Should().Be("Main organization 1");
        publication.Organizations[0].Units.Should().BeNullOrEmpty();
        publication.Organizations[1].Id.Should().Be("organizationId 2");
        publication.Organizations[1].NameFi.Should().Be("Main organization 2");
        publication.Organizations[1].Units.Should().NotBeNullOrEmpty();
        publication.Organizations[1].Units[0].Id.Should().Be("localOrganizationUnitId 999");
        publication.Organizations[1].Units[0].NameFi.Should().Be("Unit of organization 2");

        // Check that HandleEmptyCollections works correctly
        publication.Keywords.Should().BeNullOrEmpty();

        // Check that ISBN and ISSN are set correctly in HandleIssnAndIsbn
        publication.Isbn.Should().NotBeNullOrEmpty();
        publication.Isbn[0].Should().Be("ISBN 1");
        publication.Isbn[1].Should().Be("ISBN 2");
        publication.Issn.Should().NotBeNullOrEmpty();
        publication.Issn[0].Should().Be("ISSN 1");
        publication.Issn[1].Should().Be("ISSN 2");
        
        // Check that ResearchfiUrl is set correctly in HandleResearchfiUrl
        publication.ResearchfiUrl.Should().NotBeNull();
        publication.ResearchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/publication/abc234");
        publication.ResearchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/publication/abc234");
        publication.ResearchfiUrl.En.Should().Be("https://research.fi/en/results/publication/abc234");
    }
    
    
    private static Publication GetModel()
    {
        return new Publication
        {
            Id = "abc234",
            DatabasePeerReviewed = true,
            ParentPublicationName = "Parent publication name",
            ParentPublicationType = new ReferenceData() {
                Code = "code publication type",
                NameFi = "name publication type name FI",
                NameSv = "name publication type name SV",
                NameEn = "name publication type name EN"
            },
            ParentPublicationPublisher = "Parent publication publisher",
            DatabaseContributions = new List<FactContribution>()
            {
                new() {
                    OrganizationId = 123, 
                    Name = new Name()
                    {
                        NameId = 456,
                        FirstNames = "Author 1 first name",
                        LastName = "Author 1 last name",
                        Orcid = "Author 1 ORCID ID"
                    },
                    ArtPublicationRole = new ReferenceData () 
                    {
                        Code = "code 1",
                        NameFi = "name 1 FI",
                        NameSv = "name 1 SV",
                        NameEn = "name 1 EN"
                    },
                    ContributionType = "publication_author_organization"
                },
                new() {
                    OrganizationId = 345, 
                    Name = new Name()
                    {
                        NameId = 567,
                        FirstNames = "Author 2 first name",
                        LastName = "Author 2 last name",
                        Orcid = "Author 2 ORCID ID"
                    },
                    ArtPublicationRole = new ReferenceData () 
                    {
                        Code = "code 2",
                        NameFi = "name 2 FI",
                        NameSv = "name 2 SV",
                        NameEn = "name 2 EN"
                    },
                    ContributionType = "publication_author_organization_unit"
                },
                new() {
                    OrganizationId = 123, 
                    ContributionType = "publication_organization"
                },
                new() {
                    OrganizationId = 345, 
                    ContributionType = "publication_organization_unit"
                }
            },
            Isbn1 = "ISBN 1",
            Isbn2 = "ISBN 2",
            Issn1 = "ISSN 1",
            Issn2 = "ISSN 2",
            // ResearchfiUrl should be set in SetResearchfiUrl
        };
    }
}