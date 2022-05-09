using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingDecision;
using Api.Test.TestHelpers;
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

        [Fact]
        public void ShouldMapMembers_EU()
        {
            // Arrange
            var source = GetEuSource();

            // Act
            var destination = Act_Map(source);

            // Assert
            var expected = GetEuDestination();

            destination.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(FrameworkProgrameTestCases))]
        public void ShouldMapMembers_FrameworkProgramme(string expectedFrameworkProgrammeName, DimCallProgramme callProgramme)
        {
            // Arrange
            var source = GetSource();
            source.DimCallProgramme = callProgramme;

            // Act
            var destination = Act_Map(source);

            // Assert
            destination.FrameworkProgramme.Should().NotBeNull();
            destination.FrameworkProgramme.NameFi.Should().Be(expectedFrameworkProgrammeName);
        }

        public static IEnumerable<object[]> FrameworkProgrameTestCases()
        {
            var testCases = new Dictionary<string, DimCallProgramme>
            {
                ["framework 1"] = new DimCallProgramme()
                    .With(new DimCallProgramme { NameFi = "framework 1"}),
                ["framework 2"] = new DimCallProgramme()
                    .With(new DimCallProgramme { NameFi = "framework 1" }
                    .With(new DimCallProgramme { NameFi = "framework 2" })),
                ["framework 3"] = new DimCallProgramme()
                    .With(new DimCallProgramme { NameFi = "framework 1" }
                    .With(new DimCallProgramme { NameFi = "framework 2" }
                    .With(new DimCallProgramme { NameFi = "framework 3" }))),
                ["framework 4"] = new DimCallProgramme()
                    .With(new DimCallProgramme { NameFi = "framework 1" }
                    .With(new DimCallProgramme { NameFi = "framework 2" }
                    .With(new DimCallProgramme { NameFi = "framework 3" }
                    .With(new DimCallProgramme { NameFi = "framework 4" })))),
                ["framework 5"] = new DimCallProgramme()
                    .With(new DimCallProgramme { NameFi = "framework 1" }
                    .With(new DimCallProgramme { NameFi = "framework 2" }
                    .With(new DimCallProgramme { NameFi = "framework 3" }
                    .With(new DimCallProgramme { NameFi = "framework 4" }
                    .With(new DimCallProgramme { NameFi = "framework 5" }))))),
                ["framework 6"] = new DimCallProgramme()
                    .With(new DimCallProgramme { NameFi = "framework 1" }
                    .With(new DimCallProgramme { NameFi = "framework 2" }
                    .With(new DimCallProgramme { NameFi = "framework 3" }
                    .With(new DimCallProgramme { NameFi = "framework 4" }
                    .With(new DimCallProgramme { NameFi = "framework 5" }
                    .With(new DimCallProgramme { NameFi = "framework 6" })))))),
            };

            foreach (var testCase in testCases)
            {
                yield return new object[] { testCase.Key, testCase.Value };
            }


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
                DimCallProgramme = new()
                {
                    SourceId = "call programme id",
                    NameFi = "call programme fi",
                    NameSv = "call programme sv",
                    NameEn = "call programme en",
                    DimCallProgrammeId2s = new []
                    {
                        new DimCallProgramme
                        {
                            NameFi = "framework fi",
                            NameSv = "framework sv",
                            NameEn = "framework en",
                            DimCallProgrammeId2s = System.Array.Empty<DimCallProgramme>()
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
                        BusinessId = "business id",
                        RoleInConsortium = "partner",
                        ShareOfFundingInEur = 202,
                        Pic = "org pic",
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
                FrameworkProgramme = new()
                {
                    NameFi = "framework fi",
                    NameSv = "framework sv",
                    NameEn = "framework en"
                }
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
}
