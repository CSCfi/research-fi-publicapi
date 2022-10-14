using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Linq;
using CSC.PublicApi.DatabaseContext.Entities;
using Xunit;
using FundingDecisionProfile = CSC.PublicApi.DataAccess.Maps.FundingDecisionProfile;
using CSC.PublicApi.Service.Models.FundingDecision;

namespace CSC.PublicApi.Tests.Maps;

public class FundingDecisionProfileTest
{
    private readonly IMapper _mapper;

    public FundingDecisionProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<FundingDecisionProfile>()).CreateMapper();
    }

    [Fact]
    public void ConfigurationShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ShouldMapMembers()
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
    public void ShouldMapMembers_EU()
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
    public void ShouldMapMembers_WithoutUndefinedFieldOfSciences()
    {
        // Arrange
        var source = GetSource();
        source.DimFieldOfSciences = new[]
        {
            new DimFieldOfScience() { Id = -1, NameFi = "undefined"},
            new DimFieldOfScience() { Id = 1, NameFi = "first"},
            new DimFieldOfScience() { Id = 2, NameFi = "second"},
        };

        // Act
        var destination = Act_Map(source);

        // Assert
        destination.FieldsOfScience.Should().HaveCount(2);
        destination.FieldsOfScience.Should().OnlyContain(fieldOfScience => fieldOfScience.NameFi == "first" || fieldOfScience.NameFi == "second");
    }

    [Fact]
    public void ShouldMapMembers_WithoutUndefinedDates()
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
            DimFieldOfSciences = new[]
            {
                new DimFieldOfScience
                {
                    FieldId = "abc",
                    NameFi = "field fi",
                    NameSv = "field sv",
                    NameEn = "field en",
                }
            },
            DimKeywords = new[]
            {
                new DimKeyword
                {
                    Keyword = "keyword 1",
                    Scheme = "Tutkimusala"
                },
                new DimKeyword
                {
                    Keyword = "keyword 2",
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
            FundingEndDate = new System.DateTime(1988, 2, 20),
            FundingGroupPerson = new[]
            {
                new FundingGroupPerson
                {
                    FirstNames = "first names",
                    LastName = "lastname",
                    OrcId = "some orcid",
                    RoleInFundingGroup = "leader"
                }
            },
            OrganizationConsortia = new[]
            {
                new OrganizationConsortium
                {
                    NameFi = "namefi",
                    NameSv = "namesv",
                    NameEn = "nameen",
                    RoleInConsortium = "partner",
                    ShareOfFundingInEur = 202,
                    Ids = new List<Id>
                    {
                        new Id
                        {
                            Content = "business id",
                            Type = "BusinessID"
                        },
                        new Id
                        {
                            Content = "org pic",
                            Type = "PIC"
                        }
                    },
                    IsFinnishOrganization = true
                }
            },
            Funder = new()
            {
                NameFi = "funder fi",
                NameSv = "funder sv",
                NameEn = "funder en",
                Ids = new[]
                {
                    new Id { Type = "BusinessID", Content = "123"},
                    new Id { Type = "PIC", Content = "456"},
                }
            },
            TypeOfFunding = new()
            {
                NameFi = "type fi",
                NameSv = "type sv",
                NameEn = "type en",
                TypeId = "type id"
            },
            CallProgramme = new()
            {
                CallProgrammeId = "call programme id",
                NameFi = "call programme fi",
                NameSv = "call programme sv",
                NameEn = "call programme en"
            },
            CallProgrammeParent1 = new()
            {
                NameFi = "parent 1"
            },
            CallProgrammeParent2 = new()
            {
                NameFi = "parent 2"
            },
            CallProgrammeParent3 = new()
            {
                NameFi = "parent 3"
            },
            CallProgrammeParent4 = new()
            {
                NameFi = "parent 4"
            },
            CallProgrammeParent5 = new()
            {
                NameFi = "parent 5"
            },
            CallProgrammeParent6 = new()
            {
                NameFi = "parent 6"
            },
            FunderProjectNumber = "funder project number",
            FieldsOfScience = new[]
            {
                new FieldOfScience
                {
                    FieldId = "abc",
                    NameFi = "field fi",
                    NameSv = "field sv",
                    NameEn = "field en",
                }
            },
            Keywords = new[]
            {
                "keyword 1",
                "keyword 2"
            },
            AmountInEur = 123.456m,
            Topic = null,
            IdentifiedTopics = new[]
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
        fundingDecision.Topic = new()
        {
            NameFi = "call programme fi",
            NameSv = "call programme sv",
            NameEn = "call programme en",
            TopicId = "topic id",
            EuCallId = "eu call id"
        };
        return fundingDecision;
    }


}