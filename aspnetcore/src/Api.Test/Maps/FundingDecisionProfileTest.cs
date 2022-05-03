using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingDecision;
using AutoMapper;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Api.Test.Maps
{
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
            destination.Should().BeEquivalentTo(expected);
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
                DimDateIdEndNavigation = new DimDate { Year = 1988 },
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
                        RoleInFundingGroup = "leader"
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
                    NameEn = "funder en"
                },
                DimTypeOfFunding = new()
                {
                    NameFi = "type fi",
                    NameSv = "type sv",
                    NameEn = "type en",
                    TypeId = "type id"
                },
                DimCallProgramme = new()
                {
                    SourceId = "call programme id",
                    NameFi = "call programme fi",
                    NameSv = "call programme sv",
                    NameEn = "call programme en",
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
                FundingStartYear = 1987,
                FundingEndYear = 1988,
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
                        BusinessId = "business id",
                        RoleInConsortium = "partner",
                        ShareOfFundingInEur = 202,
                        Pic = "org pic"
                    }
                },
                Funder = new()
                {
                    NameFi = "funder fi",
                    NameSv = "funder sv",
                    NameEn = "funder en"
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
                    CallProgrameId = "call programme id",
                    NameFi = "call programme fi",
                    NameSv = "call programme sv",
                    NameEn = "call programme en",
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
                }
            };
        }
    }
}
