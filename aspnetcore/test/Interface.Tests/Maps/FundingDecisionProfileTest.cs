using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.Service.Models;
using FluentAssertions;
using ResearchFi.CodeList;
using Xunit;
using FrameworkProgramme = ResearchFi.FundingDecision.FrameworkProgramme;
using FundingDecision = ResearchFi.FundingDecision.FundingDecision;
using FundingReceiver = ResearchFi.FundingDecision.FundingReceiver;
using Organization = ResearchFi.FundingDecision.Organization;
using PersistentIdentifier =  ResearchFi.PersistentIdentifier;
using Keyword = ResearchFi.Keyword;
using Person = ResearchFi.FundingDecision.Person;
using Funder = ResearchFi.FundingDecision.Funder;

namespace CSC.PublicApi.Interface.Tests.Maps;

public class FundingDecisionProfileTest
{
    private readonly IMapper _mapper;

    public FundingDecisionProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.FundingDecisionProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
    
    [Fact]
    public void Map_FundingDecisionServiceModel_ReturnsApiModel()
    {
        // Arrange
        var serviceModel = GetServiceModel();   
        var apiModel = GetApiModel();

        // Act
        var result = _mapper.Map<FundingDecision>(serviceModel);

        // Assert   
        result.Should().BeEquivalentTo(apiModel);
    }

    private object GetApiModel()
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
            FundingReceivers = new List<FundingReceiver>
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
                },
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
            Funder = new Funder
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
                }
            },
            TypeOfFunding = new FundingType
            {
                NameFi = "type fi",
                NameSv = "type sv",
                NameEn = "type en",
                Code = "type id"
            },
            FrameworkProgramme = new FrameworkProgramme
            {
                NameFi = "framework programme fi",
                NameSv = "framework programme sv",
                NameEn = "framework programme en"
            },
            FunderProjectNumber = "funder project number",
            FieldsOfScience = new List<FieldOfScience>
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
                },
                new()
                {
                    Value ="keyword 2",
                    Language = "finnish",
                }
            },
            AmountInEur = 123.456m,
            IdentifiedTopics = new List<string>
            {
                "topic 1",
                "topic 2"
            }
        };
    }

    private object GetServiceModel()
    {
        return new CSC.PublicApi.Service.Models.FundingDecision.FundingDecision()
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
            FundingReceivers = new List<CSC.PublicApi.Service.Models.FundingDecision.FundingReceiver>
            {
                new()
                {
                    Person = null,
                    RoleInFundingGroup = "partner",
                    Organization = new CSC.PublicApi.Service.Models.FundingDecision.Organization
                    {
                        NameFi = "Foreign organization",
                        Pids = new List<CSC.PublicApi.Service.Models.PersistentIdentifier>()
                    },
                    ShareOfFundingInEur = 123
                },
                new()
                {
                    Person = new CSC.PublicApi.Service.Models.FundingDecision.Person
                    {
                        FirstNames = "first names",
                        LastName = "lastname",
                        OrcId = "some orcid"
                    },
                    RoleInFundingGroup = "leader",
                    ShareOfFundingInEur = 456,
                    Organization = new CSC.PublicApi.Service.Models.FundingDecision.Organization
                    {
                        NameFi = "suomalainen organisaatio",
                        NameEn = "finnish organization",
                        NameSv = "finsk organization",
                        Pids = new List<CSC.PublicApi.Service.Models.PersistentIdentifier>
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
                        }
                    }
                }
            },
            Funder = new Service.Models.FundingDecision.Organization
            {
                NameFi = "funder fi",
                NameSv = "funder sv",
                NameEn = "funder en",
                Pids = new List<CSC.PublicApi.Service.Models.PersistentIdentifier>
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
            CallProgramme = new CSC.PublicApi.Service.Models.FundingDecision.CallProgramme
            {
                SourceId = "call programme id",
                NameFi = "call programme fi",
                NameSv = "call programme sv",
                NameEn = "call programme en"
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
            Keywords = new List<CSC.PublicApi.Service.Models.Keyword>
            {
                new()
                {
                    Value ="keyword 1",
                    Language = "english",
                },
                new()
                {
                    Value ="keyword 2",
                    Language = "finnish",
                }
            },
            AmountInEur = 123.456m,
            IdentifiedTopics = new List<string>
            {
                "topic 1",
                "topic 2"
            },
            FrameworkProgramme = new CSC.PublicApi.Service.Models.FundingDecision.FrameworkProgramme()
            {
                NameFi = "framework programme fi",
                NameSv = "framework programme sv",
                NameEn = "framework programme en"
            }
        };
    }
}