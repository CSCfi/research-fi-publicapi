using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Publication;
using FluentAssertions;
using Xunit;
using FactContribution = CSC.PublicApi.DatabaseContext.Entities.FactContribution;
using Publication = CSC.PublicApi.Service.Models.Publication.Publication;

namespace CSC.PublicApi.Indexer.Tests.Maps;

public class PublicationProfileTest
{
    private readonly IMapper _mapper;

    public PublicationProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<PublicationProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ProjectTo_DimPublication_ShouldBeMappedToPublication()
    {
        // Arrange
        var entity = GetEntity();
        var model = GetModel();

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options);
    }

    private Publication Act_Map(DimPublication dbEntity)
    {
        var entityQueryable = new List<DimPublication>
        {
            dbEntity
        }.AsQueryable();

        var modelCollection = _mapper.ProjectTo<Publication>(entityQueryable);

        return modelCollection.Single();
    }

    private static DimPublication GetEntity()
    {
        return new DimPublication
        {
            Id = 1,
            PublicationId = "publicationId",
            PublicationName = "nameFi",
            PublicationYear = 2021,
            ReportingYear = 2022,
            ApcPaymentYear = 2023,
            AuthorsText = "authorsText",
            PublicationTypeCode2Navigation = new DimReferencedatum
            {
                CodeValue = "formatCode",
                NameFi = "formatNameFi",
                NameSv = "formatNameSv",
                NameEn = "formatNameEn"
            },
            ParentPublicationTypeCodeNavigation = new DimReferencedatum
            {
                CodeValue = "parentPublicationTypeCode",
                NameFi = "parentPublicationTypeNameFi",
                NameSv = "parentPublicationTypeNameSv",
                NameEn = "parentPublicationTypeNameEn"
            },
            PeerReviewed = true,
            TargetAudienceCodeNavigation = new DimReferencedatum
            {
                CodeValue = "targetAudienceCode",
                NameFi = "targetAudienceNameFi",
                NameSv = "targetAudienceNameSv",
                NameEn = "targetAudienceNameEn"
            },
            //PublicationTypeCode = "publicationTypeCode",
            PublicationTypeCodeNavigation = new DimReferencedatum
            {
                CodeValue = "publicationTypeCode",
                NameFi = "publicationTypeCodeFi",
                NameSv = "publicationTypeCodeSv",
                NameEn = "publicationTypeCodeEn"
            },
            JournalName = "JournalName",
            IssueNumber = "IssueNumber",
            ConferenceName = "ConferenceName",
            Issn = "issn",
            Issn2 = "issn2",
            Volume = "volume",
            PageNumberText = "pageNumberText",
            ArticleNumberText = "articleNumberText",
            ParentPublicationName = "parentPublicationName",
            ParentPublicationPublisher = "parentPublicationPublisher",
            Isbn = "isbn",
            Isbn2 = "isbn2",
            PublisherName = "publisherName",
            PublisherLocation = "publisherLocation",
            JufoCode = "jufoCode",
            JufoClassCode = "jufoClassCode",
            Doi = "doi",
            DoiHandle = "doiHandle",
            DimReferencedata = new List<DimReferencedatum>
            {
                new()
                {
                    CodeValue = "artPublicationTypeCategory",
                    NameFi = "artPublicationTypeNameFi",
                    NameSv = "artPublicationTypeNameSv",
                    NameEn = "artPublicationTypeNameEn"
                }
            },
            DimKeywords = new List<DimKeyword>
            {
                new()
                {
                    Language = "keywordLanguage",
                    Scheme = "keywordScheme",
                    Keyword = "keywordValue"
                }
            },
            FactContributions = new List<FactContribution>
            {
                new()
                {
                    DimOrganizationId = 42,
                    DimName = new DimName
                    {
                        Id = 23,
                        FirstNames = "personFirstName",
                        LastName = "personLastName",
                        FullName = "personFullName",
                        DimKnownPersonIdConfirmedIdentityNavigation = new DimKnownPerson
                        {
                            DimPids = new List<DimPid>
                            {
                                new()
                                {
                                    PidContent = "pidContent",
                                    PidType = "orcid"
                                }
                            }
                        },
                    },
                    DimReferencedataActorRole = new DimReferencedatum
                    {
                        CodeValue = "roleCode",
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    },
                    ContributionType = "publication_author_organization"
                },
                new()
                {
                    DimOrganizationId = 41,
                    ContributionType = "publication_organization"
                }
            },
            FactDimReferencedataFieldOfSciences = new List<FactDimReferencedataFieldOfScience>
            {
                new()
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeValue = "fieldOfScienceFieldId",
                        NameFi = "fieldOfScienceNameFi",
                        NameSv = "fieldOfScienceNameSv",
                        NameEn = "fieldOfScienceNameEn"
                    }
                }
            },
            LanguageCodeNavigation = new DimReferencedatum
            {
                CodeValue = "languageCode",
                NameFi = "languageCodeFi",
                NameSv = "languageCodeSv",
                NameEn = "languageCodeEn"
            },
            InternationalCollaboration = true,
            BusinessCollaboration = true,
            ApcFeeEur = 123.4m,
            ArticleTypeCodeNavigation = new DimReferencedatum
            {
                CodeValue = "articleTypeCode",
                NameEn = "articleTypeCodeNameEn",
                NameFi = "articleTypeCodeNameFi",
                NameSv = "articleTypeCodeNameSv",
            },
            PublicationStatusCode = "publicationStatusCode",
            LicenseCodeNavigation = new DimReferencedatum
            {
                CodeValue = "1337",
                NameEn = "licenseCodeNameEn",
                NameFi = "licenseCodeNameFi",
                NameSv = "licenseCodeNameSv",
            },
            DimLocallyReportedPubInfos = new List<DimLocallyReportedPubInfo>
            {
                new()
                {
                    SelfArchivedUrl = "selfArchivedUrl",
                    SelfArchivedLicenseCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "selfArchivedLicenseCode",
                        NameFi = "selfArchivedLicenseCodeFi",
                        NameSv = "selfArchivedLicenseCodeSv",
                        NameEn = "selfArchivedLicenseCodeEn"
                    },
                    SelfArchivedVersionCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "selfArchivedVersionCode",
                        NameFi = "selfArchivedVersionCodeFi",
                        NameSv = "selfArchivedVersionCodeSv",
                        NameEn = "selfArchivedVersionCodeEn"
                    },
                    SelfArchivedEmbargoDate = new DateTime(2023, 3, 10, 10, 40, 00),
                    SelfArchivedType = "self_archived"
                },
                new()
                {
                    SelfArchivedUrl = "preprintArchivedUrl",
                    SelfArchivedLicenseCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "preprintArchivedLicenseCode",
                        NameFi = "preprintArchivedLicenseCodeFi",
                        NameSv = "preprintArchivedLicenseCodeSv",
                        NameEn = "preprintArchivedLicenseCodeEn"
                    },
                    SelfArchivedVersionCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "preprintArchivedVersionCode",
                        NameFi = "preprintArchivedVersionCodeFi",
                        NameSv = "preprintArchivedVersionCodeSv",
                        NameEn = "preprintArchivedVersionCodeEn"
                    },
                    SelfArchivedEmbargoDate = new DateTime(2023, 3, 10, 10, 41, 00),
                    SelfArchivedType = "preprint"
                }
            },
            Abstract = "abstract",
            Created = new DateTime(2023, 3, 10, 10, 43, 00),
            Modified = new DateTime(2023, 3, 10, 10, 44, 00)
        };
    }

    private Publication GetModel()
    {
        return new Publication
        {
            Id = "publicationId",
            Name = "nameFi",
            PublicationYear = new DateTime(2021, 1, 1),
            ReportingYear = new DateTime(2022, 1, 1),
            ApcPaymentYear = new DateTime(2023, 1, 1),
            AuthorsText = "authorsText",
            Format = new ReferenceData
            {
                Code = "formatCode",
                NameFi = "formatNameFi",
                NameSv = "formatNameSv",
                NameEn = "formatNameEn"
            },
            ParentPublicationType = new ReferenceData
            {
                Code = "parentPublicationTypeCode",
                NameFi = "parentPublicationTypeNameFi",
                NameSv = "parentPublicationTypeNameSv",
                NameEn = "parentPublicationTypeNameEn"
            },
            DatabasePeerReviewed = true,
            TargetAudience = new ReferenceData
            {
                Code = "targetAudienceCode",
                NameFi = "targetAudienceNameFi",
                NameSv = "targetAudienceNameSv",
                NameEn = "targetAudienceNameEn"
            },
            Type = new ReferenceData
            {
                Code = "publicationTypeCode",
                NameFi = "publicationTypeCodeFi",
                NameSv = "publicationTypeCodeSv",
                NameEn = "publicationTypeCodeEn"
            },
            JournalName = "JournalName",
            IssueNumber = "IssueNumber",
            ConferenceName = "ConferenceName",
            Issn1 = "issn",
            Issn2 = "issn2",
            Volume = "volume",
            PageNumberText = "pageNumberText",
            ArticleNumberText = "articleNumberText",
            ParentPublicationName = "parentPublicationName",
            ParentPublicationPublisher = "parentPublicationPublisher",
            Isbn1 = "isbn",
            Isbn2 = "isbn2",
            PublisherName = "publisherName",
            PublisherLocation = "publisherLocation",
            JufoCode = "jufoCode",
            JufoClass = new ReferenceData
            {
                Code = "jufoClassCode"
            },
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
                    Value = "keywordValue",
                    Scheme = "keywordScheme"
                }
            },
            DatabaseContributions = new List<Service.Models.Publication.FactContribution>
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
                    ArtPublicationRole = new ReferenceData
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
            Language = new ReferenceData
            {
                Code = "languageCode",
                NameFi = "languageCodeFi",
                NameSv = "languageCodeSv",
                NameEn = "languageCodeEn"
            },
            InternationalPublication = false,
            InternationalCollaboration = true,
            BusinessCollaboration = true,
            ApcFeeEur = 123.4m,
            ArticleType = new ReferenceData
            {
                Code = "articleTypeCode",
                NameEn = "articleTypeCodeNameEn",
                NameFi = "articleTypeCodeNameFi",
                NameSv = "articleTypeCodeNameSv",
            },
            Status = new ReferenceData
            {
                Code = "publicationStatusCode"
            },
            License = new ReferenceData()
            {
                Code = "1337",
                NameEn = "licenseCodeNameEn",
                NameFi = "licenseCodeNameFi",
                NameSv = "licenseCodeNameSv",
            },
            Preprint = new List<LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "preprintArchivedUrl",
                    License = new ReferenceData
                    {
                        Code = "preprintArchivedLicenseCode",
                        NameFi = "preprintArchivedLicenseCodeFi",
                        NameSv = "preprintArchivedLicenseCodeSv",
                        NameEn = "preprintArchivedLicenseCodeEn"
                    },
                    Version = new ReferenceData
                    {
                        Code = "preprintArchivedVersionCode",
                        NameFi = "preprintArchivedVersionCodeFi",
                        NameSv = "preprintArchivedVersionCodeSv",
                        NameEn = "preprintArchivedVersionCodeEn"
                    },
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 41, 00)
                }
            },
            SelfArchived = new List<LocallyReportedPublicationInformation>
            {
                new()
                {
                    Url = "selfArchivedUrl",
                    License = new ReferenceData
                    {
                        Code = "selfArchivedLicenseCode",
                        NameFi = "selfArchivedLicenseCodeFi",
                        NameSv = "selfArchivedLicenseCodeSv",
                        NameEn = "selfArchivedLicenseCodeEn"
                    },
                    Version = new ReferenceData
                    {
                        Code = "selfArchivedVersionCode",
                        NameFi = "selfArchivedVersionCodeFi",
                        NameSv = "selfArchivedVersionCodeSv",
                        NameEn = "selfArchivedVersionCodeEn"
                    },
                    EmbargoDate = new DateTime(2023, 3, 10, 10, 40, 00)
                }
            },
            FieldOfArt = new ReferenceData()
            {
                Code = "123",
                NameEn = "fieldOfArtCodeNameEn",
                NameFi = "fieldOfArtCodeNameFi",
                NameSv = "fieldOfArtCodeNameSv",
            },
            Abstract = "abstract",
            ArtPublicationTypeCategory = new List<ReferenceData>
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