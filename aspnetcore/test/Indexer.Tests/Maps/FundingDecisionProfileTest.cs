﻿using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingDecision;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using FundingDecisionProfile = CSC.PublicApi.Repositories.Maps.FundingDecisionProfile;
using Keyword = CSC.PublicApi.Service.Models.Keyword;
using Organization = CSC.PublicApi.Service.Models.FundingDecision.Organization;

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
        destination.Should().BeEquivalentTo(expected);
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
            destination.FundingStartDate.Should().BeNull();
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
            DimDateIdStartNavigation = new DimDate { Year = 1987, Month = 1, Day = 1 },
            DimDateIdEndNavigation = new DimDate { Year = 1988, Month = 2, Day = 20 },
            BrParticipatesInFundingGroups = new List<BrParticipatesInFundingGroup>
            {
                new ()
                {
                    RoleInFundingGroup = "partner",
                    DimOrganization = new DimOrganization
                    {
                        NameFi = "Foreign organization"
                    },
                    ShareOfFundingInEur = 123
                }
            },
            DimFundingDecisionIdParentDecisionNavigation = new DimFundingDecision
            {
                BrParticipatesInFundingGroups = new List<BrParticipatesInFundingGroup>
                {
                    new ()
                    {
                        DimName = new DimName
                        {
                            LastName = "lastname",
                            FirstNames = "first names",
                            DimKnownPersonIdConfirmedIdentityNavigation = new DimKnownPerson
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
                        DimOrganization = new DimOrganization
                        {
                            NameFi = "suomalainen organisaatio",
                            NameEn = "finnish organization",
                            NameSv = "finsk organization",
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
                        ShareOfFundingInEur = 456
                    }
                }
            },
            BrFundingConsortiumParticipations = new[]
            {
                new BrFundingConsortiumParticipation
                {
                    DimOrganization = new DimOrganization
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
            DimOrganizationIdFunderNavigation = new DimOrganization
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
            DimTypeOfFunding = new DimTypeOfFunding
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
                    new()
                    {
                        NameFi = "parent 1",
                        DimCallProgrammeId2s = new List<DimCallProgramme>
                        {
                            new()
                            {
                                NameFi = "parent 2",
                                DimCallProgrammeId2s = new List<DimCallProgramme>
                                {
                                    new()
                                    {
                                        NameFi = "parent 3",
                                        DimCallProgrammeId2s = new List<DimCallProgramme>
                                        {
                                            new()
                                            {
                                                NameFi = "parent 4",
                                                DimCallProgrammeId2s = new List<DimCallProgramme>
                                                {
                                                    new()
                                                    {
                                                        NameFi = "parent 5",
                                                        DimCallProgrammeId2s = new List<DimCallProgramme>
                                                        {
                                                            new()
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
                    DimWordCluster = new DimWordCluster
                    {
                        BrWordsDefineAClusters = new[]
                        {
                            new BrWordsDefineACluster
                            {
                                DimMinedWords = new DimMinedWord
                                {
                                    Word = "topic 1"
                                }
                            },
                            new BrWordsDefineACluster
                            {
                                DimMinedWords = new DimMinedWord
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
            FundingStartDate = new DateTime(1987, 1,1),
            FundingEndDate = new DateTime(1988, 2, 20),
            SelfFundingGroupPerson = new List<FundingGroupPerson>
            {
                new()
                {
                    Person = null,
                    RoleInFundingGroup = "partner",
                    Organization = new Organization
                    {
                        NameFi = "Foreign organization",
                        Pids = new List<PersistentIdentifier>(),
                        IsFinnishOrganization = false
                    },
                    ShareOfFundingInEur = 123
                }
            },
            ParentFundingGroupPerson = new List<FundingGroupPerson>
            {
                new()
                {
                    Person = new Person
                    {
                        FirstNames = "first names",
                        LastName = "lastname",
                        OrcId = "some orcid"
                    },
                    RoleInFundingGroup = "leader",
                    ShareOfFundingInEur = 456,
                    Organization = new Organization
                    {
                        NameFi = "suomalainen organisaatio",
                        NameEn = "finnish organization",
                        NameSv = "finsk organization",
                        Pids = new List<PersistentIdentifier>
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
                }
            },
            OrganizationConsortia = new List<OrganizationConsortium>
            {
                new()
                {
                    Organization = new Organization
                    {
                        NameFi = "namefi",
                        NameSv = "namesv",
                        NameEn = "nameen",
                        Pids = new List<PersistentIdentifier>
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
                    },
                    RoleInConsortium = "partner",
                    ShareOfFundingInEur = 202
                }
            },
            Funder = new Organization
            {
                NameFi = "funder fi",
                NameSv = "funder sv",
                NameEn = "funder en",
                Pids = new List<PersistentIdentifier>
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
                },
                IsFinnishOrganization = true
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
                SourceId = "call programme id",
                NameFi = "call programme fi",
                NameSv = "call programme sv",
                NameEn = "call programme en"
            },
            CallProgrammeParent1 = new CallProgramme()
            {
                NameFi = "parent 1"
            },
            CallProgrammeParent2 = new CallProgramme
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
}