using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Publication;
using CSC.PublicApi.Service.Models.Publication;
using FluentAssertions;
using Xunit;
using Author = CSC.PublicApi.Interface.Models.Publication.Author;
using LocallyReportedPublicationInformation = CSC.PublicApi.Interface.Models.Publication.LocallyReportedPublicationInformation;
using Organization = CSC.PublicApi.Interface.Models.Publication.Organization;
using ParentPublication = CSC.PublicApi.Interface.Models.Publication.ParentPublication;
using Publication = CSC.PublicApi.Interface.Models.Publication.Publication;

namespace CSC.PublicApi.Interface.Tests.Maps;

public class PublicationProfileTest
{
    private readonly IMapper _mapper;

    public PublicationProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.PublicationProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
    
    [Fact]
    public void Map_PublicationServiceModel_ReturnsApiModel()
    {
        // Arrange
        var serviceModel = GetServiceModel();
        var apiModel = GetApiModel();

        // Act
        var result = _mapper.Map<Publication>(serviceModel);

        // Assert
        result.Should().BeEquivalentTo(apiModel);
    }

    [Fact]
    public void Map_GetPublicationQueryParameters_ConvertsListsToArrays()
    {
        // Arrange
        var apiModel = new GetPublicationsQueryParameters
        {
            Keywords = "keywords",
            Name = "name",

        };
        
        var elasticModel = new PublicationSearchParameters
        {
            Keywords = "keywords",
            Name = "name",
        };

        // Act
        var result = _mapper.Map<PublicationSearchParameters>(apiModel);

        // Assert
        result.Should().BeEquivalentTo(elasticModel);
    }

    private static object GetServiceModel()
    {
        return new Service.Models.Publication.Publication
        {
            Id = "publicationId",
            Name = "nameFi",
            PublicationYear = new DateTime(2021,1,1),
            ReportingYear = new DateTime(2022,1,1),
            ApcPaymentYear = new DateTime(2023,1,1),
            AuthorsText = "authorsText",
            Format = new Service.Models.ReferenceData
            {
                Code = "formatCode",
                NameFi = "formatNameFi",
                NameSv = "formatNameSv",
                NameEn = "formatNameEn"
            },
            PeerReviewed = new Service.Models.ReferenceData
            {
                Code = "peerReviewedCode",
                NameFi = "peerReviewedNameFi",
                NameSv = "peerReviewedNameSv",
                NameEn = "peerReviewedNameEn"
            },
            TargetAudience = new Service.Models.ReferenceData
            {
                Code = "targetAudienceCode",
                NameFi = "targetAudienceNameFi",
                NameSv = "targetAudienceNameSv",
                NameEn = "targetAudienceNameEn"
            },
            TypeCode = "publicationTypeCode",
            JournalName = "JournalName",
            IssueNumber = "IssueNumber",
            ConferenceName = "ConferenceName",
            Issn = new List<string?>
            {
                "issn",
                "issn2"                
            },
            Volume = "volume",
            PageNumberText = "pageNumberText",
            ArticleNumberText = "articleNumberText",
            ParentPublication = new Service.Models.Publication.ParentPublication
            {
                Name = "parentPublicationName",
                Publisher = "parentPublicationPublisher",
                Type = new Service.Models.ReferenceData
                {
                    Code = "parentPublicationTypeCode",
                    NameFi = "parentPublicationTypeNameFi",
                    NameSv = "parentPublicationTypeNameSv",
                    NameEn = "parentPublicationTypeNameEn"                    
                }
            },
            Isbn = new List<string?>
            {
                "isbn",
                "isbn2",
            },
            PublisherName = "publisherName",
            PublisherLocation = "publisherLocation",
            JufoCode = "jufoCode",
            JufoClassCode = "jufoClassCode",
            Doi = "doi",
            DoiHandle = "doiHandle",
            FieldsOfEducation = new List<Service.Models.ReferenceData>
            {
                new()
                {
                    Code = "foeFieldId",
                    NameFi = "foeNameFi",
                    NameSv = "foeNameSv",
                    NameEn = "foeNameEn"
                }
            },
            Keywords = new List<Service.Models.Keyword>
            {
                new()
                {
                    Language = "keywordLanguage",
                    Value = "keywordValue"
                }
            },
            Organizations = new List<Service.Models.Publication.Organization>
            {
                new()
                {
                    Id = "orgId",
                    NameFi = "Organisaatio",
                    NameSv = "organisation",
                    NameEn = "Organization",
                    Units = null
                }
            },
            Authors = new List<Service.Models.Publication.Author>
            {
                new()
                {
                    FirstNames = "first Names",
                    LastName = "lastName",
                    ArtPublicationRole = new Service.Models.ReferenceData()
                    {
                        Code = "artPublicationRole",
                        NameFi = "artPublicationNameFi",
                        NameSv = "artPublicationNameSv",
                        NameEn = "artPublicationNameEn"
                    },
                    Orcid = "orcid",
                    Organizations = null
                }
            },
            DatabaseContributions = new List<FactContribution>
            {
                new()
                {
                    OrganizationId = 42,
                    Name = new Name
                    {
                        NameId = 23,
                        FirstNames = "personFirstName",
                        LastName = "personLastName",
                        Orcid = "pidContent"
                    },
                    ArtPublicationRole = new Service.Models.ReferenceData
                    {
                        Code = "roleCode",
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    },
                    ContributionType = "publication_author_organization"
                },
                new()
                {
                    OrganizationId = 41,
                    ContributionType = "publication_organization"
                }
            },
            FieldsOfScience = new List<Service.Models.ReferenceData>
            {
                new()
                { 
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            LanguageCode = "languageCode",
            InternationalPublication = false,
            InternationalCollaboration = true,
            BusinessCollaboration = true,
            ApcFeeEur = 123.4m,
            ArticleTypeCode = new Service.Models.ReferenceData
            {
                Code = "articleTypeCode",
                NameEn = "articleTypeCodeNameEn",
                NameFi = "articleTypeCodeNameFi",
                NameSv = "articleTypeCodeNameSv",
            },
            StatusCode = "publicationStatusCode",
            LicenseId = "1337",
            Preprint = new List<Service.Models.Publication.LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "preprintArchivedUrl",
                    LicenseCode = "preprintArchivedLicenseCode",
                    VersionCode = "preprintArchivedVersionCode",
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 41, 00)
                }
            },
            SelfArchived = new List<Service.Models.Publication.LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "selfArchivedUrl",
                    LicenseCode = "selfArchivedLicenseCode",
                    VersionCode = "selfArchivedVersionCode",
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 40, 00)
                }
            },
            FieldOfArts = new List<Service.Models.ReferenceData>
            {
                new()
                {
                    Code = "fieldOfArtsId",
                    NameFi = "fieldOfArtsNameFi",
                    NameSv = "fieldOfArtsNameSv",
                    NameEn = "fieldOfArtsNameEn"
                }
            },
            Abstract = "abstract",
            ArtPublicationTypeCategory =  new List<Service.Models.ReferenceData>
            {
                new ()
                {
                    Code = "artPublicationTypeCategory",
                    NameFi = "artPublicationTypeNameFi",
                    NameSv = "artPublicationTypeNameSv",
                    NameEn = "artPublicationTypeNameEn"
                }
            },
            Created = new DateTime(2023, 3, 10, 10, 43, 00),
            Modified = new DateTime(2023, 3, 10, 10, 44, 00)
        };
    }

    private object GetApiModel()
    {
        return new Publication
        {
            Id = "publicationId",
            Name = "nameFi",
            PublicationYear = "2021",
            ReportingYear = "2022",
            ApcPaymentYear = "2023",
            AuthorsText = "authorsText",
            Format = new ReferenceData
            {
                Code = "formatCode",
                NameFi = "formatNameFi",
                NameSv = "formatNameSv",
                NameEn = "formatNameEn"
            },
            ParentPublication = new ParentPublication
            {
                Name = "parentPublicationName",
                Publisher = "parentPublicationPublisher",
                Type = new ReferenceData
                {
                    Code = "parentPublicationTypeCode",
                    NameFi = "parentPublicationTypeNameFi",
                    NameSv = "parentPublicationTypeNameSv",
                    NameEn = "parentPublicationTypeNameEn"                    
                }
            },
            PeerReviewed = new ReferenceData
            {
                Code = "peerReviewedCode",
                NameFi = "peerReviewedNameFi",
                NameSv = "peerReviewedNameSv",
                NameEn = "peerReviewedNameEn"
            },
            TargetAudience = new ReferenceData
            {
                Code = "targetAudienceCode",
                NameFi = "targetAudienceNameFi",
                NameSv = "targetAudienceNameSv",
                NameEn = "targetAudienceNameEn"
            },
            TypeCode = "publicationTypeCode",
            JournalName = "JournalName",
            IssueNumber = "IssueNumber",
            ConferenceName = "ConferenceName",
            Issn = new List<string>
            {
                "issn",
                "issn2"
            },
            Volume = "volume",
            PageNumberText = "pageNumberText",
            ArticleNumberText = "articleNumberText",
            Isbn = new List<string>
            {
                "isbn",
                "isbn2"
            },
            PublisherName = "publisherName",
            PublisherLocation = "publisherLocation",
            JufoCode = "jufoCode",
            JufoClassCode = "jufoClassCode",
            Doi = "doi",
            DoiHandle = "doiHandle",
            FieldsOfEducation = new List<ReferenceData>
            {
                new()
                {
                    Code = "foeFieldId",
                    NameFi = "foeNameFi",
                    NameSv = "foeNameSv",
                    NameEn = "foeNameEn"
                }
            },
            Keywords = new List<Keyword>
            {
                new()
                {
                    Language = "keywordLanguage",
                    Value = "keywordValue"
                }
            },
            Organizations = new List<Organization>
            {
                new()
                {
                    Id = "orgId",
                    NameFi = "Organisaatio",
                    NameSv = "organisation",
                    NameEn = "Organization",
                    Units = null
                }
            },
            Authors = new List<Author>
            {
                new()
                {
                    FirstNames = "first Names",
                    LastName = "lastName",
                    ArtPublicationRole = new ReferenceData()
                    {
                        Code = "artPublicationRole",
                        NameFi = "artPublicationNameFi",
                        NameSv = "artPublicationNameSv",
                        NameEn = "artPublicationNameEn"
                    },
                    Orcid = "orcid",
                    Organizations = null
                }
            },
            FieldsOfScience = new List<ReferenceData>
            {
                new()
                { 
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            LanguageCode = "languageCode",
            InternationalPublication = false,
            InternationalCollaboration = true,
            BusinessCollaboration = true,
            ApcFeeEur = 123.4m,
            ArticleTypeCode = new ReferenceData
            {
                Code = "articleTypeCode",
                NameEn = "articleTypeCodeNameEn",
                NameFi = "articleTypeCodeNameFi",
                NameSv = "articleTypeCodeNameSv",
            },
            StatusCode = "publicationStatusCode",
            LicenseId = "1337",
            Preprint = new List<LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "preprintArchivedUrl",
                    LicenseCode = "preprintArchivedLicenseCode",
                    VersionCode = "preprintArchivedVersionCode",
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 41, 00)
                }
            },
            SelfArchived = new List<LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "selfArchivedUrl",
                    LicenseCode = "selfArchivedLicenseCode",
                    VersionCode = "selfArchivedVersionCode",
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 40, 00)
                }
            },
            FieldOfArts = new List<ReferenceData>
            {
                new()
                {
                    Code = "fieldOfArtsId",
                    NameFi = "fieldOfArtsNameFi",
                    NameSv = "fieldOfArtsNameSv",
                    NameEn = "fieldOfArtsNameEn"
                }
            },
            Abstract = "abstract",
            ArtPublicationTypeCategory =  new List<ReferenceData>
            {
                new ()
                {
                    Code = "artPublicationTypeCategory",
                    NameFi = "artPublicationTypeNameFi",
                    NameSv = "artPublicationTypeNameSv",
                    NameEn = "artPublicationTypeNameEn"
                }
            },
            Created = new DateTime(2023, 3, 10, 10, 43, 00),
            Modified = new DateTime(2023, 3, 10, 10, 44, 00)
        };
    }
}

