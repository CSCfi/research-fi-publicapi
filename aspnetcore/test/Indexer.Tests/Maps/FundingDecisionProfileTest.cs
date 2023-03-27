using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingDecision;
using FluentAssertions;
using FluentAssertions.Execution;
using Nest;
using Xunit;
using FundingDecisionProfile = CSC.PublicApi.Repositories.Maps.FundingDecisionProfile;
using Keyword = CSC.PublicApi.Service.Models.Keyword;

namespace CSC.PublicApi.Indexer.Tests.Maps;

public class FundingDecisionProfileTest
{
    private readonly IMapper _mapper;

    public FundingDecisionProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<FundingDecisionProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ProjectTo_DimFundingDecision_ShouldBeMappedToFundingDecision()
    {
        // Arrange
        var source = GetSource();

        // Act
        var destination = Act_Map(source);

        // Assert
        var expected = GetDestination();
        destination.Should().BeEquivalentTo(expected, opt => opt.Excluding(d => d.OrganizationConsortia2));
    }

    [Fact]
    public void ProjectTo_DimFundingDecisionWithEUFunding_ShouldBeMappedToFundingDecision()
    {
        // Arrange
        var source = GetEuSource();

        // Act
        var destination = Act_Map(source);

        // Assert
        var expected = GetEuDestination();

        destination.Should().BeEquivalentTo(expected, opt => opt.Excluding(d => d.OrganizationConsortia2));
    }

    [Fact]
    public void ProjectTo_DimFundingDecisionWithoutUndefinedFieldOFSciences_ShouldBeMappedToFundingDecision()
    {
        // Arrange
        var source = GetSource();
        source.FactDimReferencedataFieldOfSciences = new List<FactDimReferencedataFieldOfScience>
        {
            new()
            {
                DimReferencedata = new DimReferencedatum
                {
                    NameFi = "first"
                }
            },
            new()
            {
                DimReferencedata = new DimReferencedatum
                {
                    NameFi = "second"
                }
            }
        };

        // Act
        var destination = Act_Map(source);

        // Assert
        destination.FieldsOfScience.Should().HaveCount(2);
        destination.FieldsOfScience.Should().OnlyContain(fieldOfScience => fieldOfScience.NameFi == "first" || fieldOfScience.NameFi == "second");
    }

    [Fact]
    public void ProjectTo_DimFundingDecisionWithoutUndefinedDates_ShouldBeMappedToFundingDecision()
    {
        // Arrange
        var source = GetSource();
        source.DimDateIdStartNavigation = new DimDate { Id = -1 };
        source.DimDateIdEndNavigation = new DimDate { Id = -1 };

        // Act
        var destination = Act_Map(source);

        // Assert
        using (new AssertionScope())
        {
            destination.FundingStartYear.Should().BeNull();
            destination.FundingEndYear.Should().BeNull();
            destination.FundingEndDate.Should().BeNull();
        }
    }

    private FundingDecision Act_Map(DimFundingDecision dbEntity)
    {
        var entityQueryable = new List<DimFundingDecision>
        {
            dbEntity
        }.AsQueryable();

        var modelCollection = _mapper.ProjectTo<FundingDecision>(entityQueryable);

        return modelCollection.Single();
    }

    private static DimFundingDecision GetSource()
    {
        return new DimFundingDecision
        {
            NameFi = "namefi",
            NameSv = "namesv",
            NameEn = "nameen",
            Acronym = "acronym",
            DescriptionFi = "desc fi",
            DescriptionSv = "desc sv",
            DescriptionEn = "desc en",
            DimDateIdStartNavigation = new DimDate { Year = 1987 },
            DimDateIdEndNavigation = new DimDate { Year = 1988, Month = 2, Day = 20 },
            BrParticipatesInFundingGroups = new List<BrParticipatesInFundingGroup>
            {
                new ()
                {
                    DimName = new ()
                    {
                        LastName = "lastname",
                        FirstNames = "first names",
                        DimKnownPersonIdConfirmedIdentityNavigation = new ()
                        {
                            DimPids = new List<DimPid>
                            {
                                new ()
                                {
                                    PidType = "Orcid",
                                    PidContent = "some orcid"
                                }
                            }
                        }
                    },
                    RoleInFundingGroup = "leader",
                    DimOrganization = new DimOrganization()
                    {
                        NameFi = "sdf"
                    }
                }
            },
            BrFundingConsortiumParticipations = new[]
            {
                new BrFundingConsortiumParticipation
                {
                    DimOrganization = new ()
                    {
                        NameFi = "namefi",
                        NameSv = "namesv",
                        NameEn = "nameen",
                        DimPids = new []
                        {
                            new DimPid
                            {
                                PidType = "BusinessID",
                                PidContent = "business id",
                            },
                            new DimPid
                            {
                                PidType = "PIC",
                                PidContent = "org pic",
                            }
                        },
                    },
                    RoleInConsortium = "partner",
                    ShareOfFundingInEur = 202
                }
            },
            DimOrganizationIdFunderNavigation = new()
            {
                NameFi = "funder fi",
                NameSv = "funder sv",
                NameEn = "funder en",
                DimPids = new[]
                {
                    new DimPid
                    {
                        PidType = "BusinessID",
                        PidContent = "123"
                    },
                    new DimPid
                    {
                        PidType = "PIC",
                        PidContent = "456"
                    }
                }
            },
            DimTypeOfFunding = new()
            {
                NameFi = "type fi",
                NameSv = "type sv",
                NameEn = "type en",
                TypeId = "type id"
            },
            DimCallProgramme = new DimCallProgramme
            {
                SourceId = "call programme id",
                NameFi = "call programme fi",
                NameSv = "call programme sv",
                NameEn = "call programme en",
                DimCallProgrammeId2s = new List<DimCallProgramme>
                {
                    new DimCallProgramme
                    {
                        NameFi = "parent 1",
                        DimCallProgrammeId2s = new List<DimCallProgramme>
                        {
                            new DimCallProgramme
                            {
                                NameFi = "parent 2",
                                DimCallProgrammeId2s = new List<DimCallProgramme>
                                {
                                    new DimCallProgramme
                                    {
                                        NameFi = "parent 3",
                                        DimCallProgrammeId2s = new List<DimCallProgramme>
                                        {
                                            new DimCallProgramme
                                            {
                                                NameFi = "parent 4",
                                                DimCallProgrammeId2s = new List<DimCallProgramme>
                                                {
                                                    new DimCallProgramme
                                                    {
                                                        NameFi = "parent 5",
                                                        DimCallProgrammeId2s = new List<DimCallProgramme>
                                                        {
                                                            new DimCallProgramme
                                                            {
                                                                NameFi = "parent 6",
                                                                DimCallProgrammeId2s = new List<DimCallProgramme>
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            FunderProjectNumber = "funder project number",
            FactDimReferencedataFieldOfSciences =new List<FactDimReferencedataFieldOfScience>
            {
                new()
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeValue = "abc",
                        NameFi = "field fi",
                        NameSv = "field sv",
                        NameEn = "field en",
                    }
                }
            },
            DimKeywords = new[]
            {
                new DimKeyword
                {
                    Keyword = "keyword 1",
                    Language = "english",
                    Scheme = "Tutkimusala"
                },
                new DimKeyword
                {
                    Keyword = "keyword 2",
                    Language = "finnish",
                    Scheme = "Tutkimusala"
                },
                new DimKeyword
                {
                    Keyword = "keyword 3",
                    Scheme = "some other scheme"
                }
            },
            AmountInEur = 123.456m,
            BrWordClusterDimFundingDecisions = new[]
            {
                new BrWordClusterDimFundingDecision
                {
                    DimWordCluster = new ()
                    {
                        BrWordsDefineAClusters = new[]
                        {
                            new BrWordsDefineACluster
                            {
                                DimMinedWords = new ()
                                {
                                    Word = "topic 1"
                                }
                            },
                            new BrWordsDefineACluster
                            {
                                DimMinedWords = new ()
                                {
                                    Word = "topic 2"
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private static DimFundingDecision GetEuSource()
    {
        var fundingDecision = GetSource();
        fundingDecision.SourceDescription = "eu_funding";
        fundingDecision.DimCallProgramme.Abbreviation = "topic id";
        fundingDecision.DimCallProgramme.EuCallId = "eu call id";

        return fundingDecision;
    }

    private static FundingDecision GetDestination()
    {
        return new FundingDecision
        {
            NameFi = "namefi",
            NameSv = "namesv",
            NameEn = "nameen",
            Acronym = "acronym",
            DescriptionFi = "desc fi",
            DescriptionSv = "desc sv",
            DescriptionEn = "desc en",
            FundingStartYear = 1987,
            FundingEndYear = 1988,
            FundingEndDate = new DateTime(1988, 2, 20),
            FundingGroupPerson = new List<FundingGroupPerson>
            {
                new()
                {
                    FirstNames = "first names",
                    LastName = "lastname",
                    OrcId = "some orcid",
                    RoleInFundingGroup = "leader"
                }
            },
            OrganizationConsortia = new List<OrganizationConsortium>
            {
                new()
                {
                    NameFi = "namefi",
                    NameSv = "namesv",
                    NameEn = "nameen",
                    RoleInConsortium = "partner",
                    ShareOfFundingInEur = 202,
                    Ids = new List<PersistentIdentifier>
                    {
                        new()
                        {
                            Content = "business id",
                            Type = "BusinessID"
                        },
                        new()
                        {
                            Content = "org pic",
                            Type = "PIC"
                        }
                    },
                    IsFinnishOrganization = true
                }
            },
            Funder = new Funder
            {
                NameFi = "funder fi",
                NameSv = "funder sv",
                NameEn = "funder en",
                Ids = new List<PersistentIdentifier>
                {
                    new()
                    {
                        Type = "BusinessID", 
                        Content = "123"
                    }, 
                    new() 
                    { 
                        Type = "PIC", 
                        Content = "456"
                    }
                }
            },
            TypeOfFunding = new ReferenceData
            {
                NameFi = "type fi",
                NameSv = "type sv",
                NameEn = "type en",
                Code = "type id"
            },
            CallProgramme = new CallProgramme
            {
                CallProgrammeId = "call programme id",
                NameFi = "call programme fi",
                NameSv = "call programme sv",
                NameEn = "call programme en"
            },
            CallProgrammeParent1 = new FrameworkProgramme
            {
                NameFi = "parent 1"
            },
            CallProgrammeParent2 = new FrameworkProgramme
            {
                NameFi = "parent 2"
            },
            CallProgrammeParent3 = new FrameworkProgramme
            {
                NameFi = "parent 3"
            },
            CallProgrammeParent4 = new FrameworkProgramme
            {
                NameFi = "parent 4"
            },
            CallProgrammeParent5 = new FrameworkProgramme
            {
                NameFi = "parent 5"
            },
            CallProgrammeParent6 = new FrameworkProgramme
            {
                NameFi = "parent 6"
            },
            FunderProjectNumber = "funder project number",
            FieldsOfScience = new List<ReferenceData>
            {
                new()
                {
                    Code = "abc",
                    NameFi = "field fi",
                    NameSv = "field sv",
                    NameEn = "field en",
                }
            },
            Keywords = new List<Keyword>
            {
                new()
                {
                    Value ="keyword 1",
                    Language = "english",
                    Scheme = "Tutkimusala"
                },
                new()
                {
                    Value ="keyword 2",
                    Language = "finnish",
                    Scheme = "Tutkimusala"
                }
            },
            AmountInEur = 123.456m,
            IdentifiedTopics = new List<string>
            {
                "topic 1",
                "topic 2"
            },
            FrameworkProgramme = null
        };
    }

    private static FundingDecision GetEuDestination()
    {
        var fundingDecision = GetDestination();
        fundingDecision.CallProgramme = null;
        fundingDecision.Topic = new Topic
        {
            TopicId = "topic id",
            NameFi = "call programme fi",
            NameSv = "call programme sv",
            NameEn = "call programme en",
            EuCallId = "eu call id"
        };
        return fundingDecision;
    }
}