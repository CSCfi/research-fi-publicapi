using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Publication;
using Xunit;
using FluentAssertions;
using ResearchFi.CodeList;
using ResearchFi.Query;
using Author = ResearchFi.Publication.Author;
using Keyword = ResearchFi.Keyword;
using LocallyReportedPublicationInformation = ResearchFi.Publication.LocallyReportedPublicationInformation;
using Organization = ResearchFi.Publication.Organization;
using ParentPublication = ResearchFi.Publication.ParentPublication;
using Publication = ResearchFi.Publication.Publication;

namespace CSC.PublicApi.Interface.Tests.Maps;

public class PublicationProfileTest
{
    private readonly IMapper _mapper;

    public PublicationProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.PublicationProfile>())
            .CreateMapper();
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
            PublicationYear = new DateTime(2021, 1, 1),
            ReportingYear = new DateTime(2022, 1, 1),
            ApcPaymentYear = new DateTime(2023, 1, 1),
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
            Type = new ReferenceData
            {
                Code = "publicationTypeCode"
            },
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
            Isbn = new List<string>
            {
                "isbn",
                "isbn2",
            },
            PublisherName = "publisherName",
            PublisherLocation = "publisherLocation",
            JufoCode = "jufoCode",
            JufoClass = new ReferenceData
            {
                Code = "jufoClassCode"
            },
            Doi = "doi",
            DoiHandle = "doiHandle",
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
            Language = new ReferenceData
            {
                Code = "languageCode"
            },
            InternationalPublication = false,
            InternationalCollaboration = true,
            BusinessCollaboration = true,
            ApcFeeEur = 123.4m,
            ArticleType = new Service.Models.ReferenceData
            {
                Code = "articleTypeCode",
                NameEn = "articleTypeCodeNameEn",
                NameFi = "articleTypeCodeNameFi",
                NameSv = "articleTypeCodeNameSv",
            },
            Status = new ReferenceData
            {
                Code = "publicationStatusCode",
                NameFi = "publicationStatusNameFi",
                NameSv = "publicationStatusNameSv",
                NameEn = "publicationStatusNameEn"
            },
            License = new ReferenceData
            {
                Code = "1337"
            },
            Preprint = new List<Service.Models.Publication.LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "preprintArchivedUrl",
                    License = new ReferenceData
                    {
                        Code = "preprintArchivedLicenseCode"
                    },
                    Version = new ReferenceData
                    {
                        Code = "preprintArchivedVersionCode"
                    },
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 41, 00)
                }
            },
            SelfArchived = new List<Service.Models.Publication.LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "selfArchivedUrl",
                    License = new ReferenceData
                    {
                        Code = "selfArchivedLicenseCode"
                    },
                    Version = new ReferenceData
                    {
                        Code = "selfArchivedVersionCode"
                    },
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 40, 00)
                }
            },
            FieldOfArt = new ReferenceData
            {
                    Code = "fieldOfArtsId",
                    NameFi = "fieldOfArtsNameFi",
                    NameSv = "fieldOfArtsNameSv",
                    NameEn = "fieldOfArtsNameEn"
            },
            Abstract = "abstract",
            ArtPublicationTypeCategory = new List<Service.Models.ReferenceData>
            {
                new()
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

    private static object GetApiModel()
    {
        return new Publication
        {
            Id = "publicationId",
            Name = "nameFi",
            PublicationYear = "2021",
            ReportingYear = "2022",
            ApcPaymentYear = "2023",
            AuthorsText = "authorsText",
            Format = new PublicationFormat
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
                Type = new ParentPublicationType
                {
                    Code = "parentPublicationTypeCode",
                    NameFi = "parentPublicationTypeNameFi",
                    NameSv = "parentPublicationTypeNameSv",
                    NameEn = "parentPublicationTypeNameEn"
                }
            },
            PeerReviewed = new PeerReviewed
            {
                Code = "peerReviewedCode",
                NameFi = "peerReviewedNameFi",
                NameSv = "peerReviewedNameSv",
                NameEn = "peerReviewedNameEn"
            },
            TargetAudience = new TargetAudience
            {
                Code = "targetAudienceCode",
                NameFi = "targetAudienceNameFi",
                NameSv = "targetAudienceNameSv",
                NameEn = "targetAudienceNameEn"
            },
            Type = new PublicationType
            {
                Code = "publicationTypeCode"
            },
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
            JufoClass = new JufoClass
            {
                Code = "jufoClassCode"
            },
            Doi = "doi",
            DoiHandle = "doiHandle",
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
                    ArtPublicationRole = new ArtPublicationRole
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
            FieldsOfScience = new List<FieldOfScience>
            {
                new()
                {
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Language = new Language
            {
                Code = "languageCode"
            },
            InternationalPublication = false,
            InternationalCollaboration = true,
            BusinessCollaboration = true,
            ApcFeeEur = 123.4m,
            ArticleType = new ArticleType
            {
                Code = "articleTypeCode",
                NameEn = "articleTypeCodeNameEn",
                NameFi = "articleTypeCodeNameFi",
                NameSv = "articleTypeCodeNameSv",
            },
            Status = new PublicationStatus
            {
                Code = "publicationStatusCode",
                NameFi = "publicationStatusNameFi",
                NameSv = "publicationStatusNameSv",
                NameEn = "publicationStatusNameEn"
            },
            License = new License
            {
                Code = "1337"
            },
            Preprint = new List<LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "preprintArchivedUrl",
                    License = new License
                    {
                        Code = "preprintArchivedLicenseCode"
                    },
                    Version = new SelfArchiveVersion
                    {
                        Code = "preprintArchivedVersionCode"
                    },
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 41, 00)
                }
            },
            SelfArchived = new List<LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "selfArchivedUrl",
                    License = new License
                    {
                        Code = "selfArchivedLicenseCode"
                    },
                    Version = new SelfArchiveVersion
                    {
                        Code = "selfArchivedVersionCode"
                    },
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 40, 00)
                }
            },
            FieldOfArt = new FieldOfArt
            {
                Code = "fieldOfArtsId",
                NameFi = "fieldOfArtsNameFi",
                NameSv = "fieldOfArtsNameSv",
                NameEn = "fieldOfArtsNameEn"
            },
            Abstract = "abstract",
            ArtPublicationTypeCategory = new List<ArtPublicationTypeCategory>
            {
                new()
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