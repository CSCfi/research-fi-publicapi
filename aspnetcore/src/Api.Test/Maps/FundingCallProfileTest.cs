using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using AutoMapper;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FundingCall = Api.Models.FundingCall.FundingCall;

namespace Api.Test.Maps
{
    public class FundingCallProfileTest
    {
        private readonly IMapper _mapper;

        public FundingCallProfileTest()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<FundingCallProfile>()).CreateMapper();
        }

        [Fact]
        public void Configuration_ShouldBeValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_DimCallProgramme_To_FundingCall()
        {
            // Arrange
            var entity = GetEntity();

            // Act
            var model = Act_Map(entity);

            // Assert
            model.Should().BeEquivalentTo(GetModel());
        }

        private FundingCall Act_Map(DimCallProgramme dbEntity)
        {
            var entityQueryable = new List<DimCallProgramme>
            {
                dbEntity
            }.AsQueryable();

            var modelCollection = _mapper.ProjectTo<FundingCall>(entityQueryable);

            return modelCollection.Single();
        }

        private static DimCallProgramme GetEntity()
        {
            return new DimCallProgramme
            {
                NameFi = "nameFi",
                NameSv = "nameSv",
                NameEn = "nameEn",
                ApplicationTermsFi = "appTermsFi",
                ApplicationTermsSv = "appTermsSv",
                ApplicationTermsEn = "appTermsEn",
                DescriptionFi = "descFi",
                DescriptionSv = "descSv",
                DescriptionEn = "descEn",
                //DimWebLinks = new[]
                //{
                //    new DimWebLink
                //    {
                //        LinkType = "ApplicationUrl",
                //        LanguageVariant = "fi",
                //        Url = "http://urlFi"
                //    },
                //    new DimWebLink
                //    {
                //        LinkType = "ApplicationUrl",
                //        LanguageVariant = "sv",
                //        Url = "http://urlSv"
                //    },
                //    new DimWebLink
                //    {
                //        LinkType = "ApplicationUrl",
                //        LanguageVariant = "en",
                //        Url = "http://urlEn"
                //    }
                //},
                DimDateIdOpenNavigation = new DimDate
                {
                    Id = 4,
                    Year = 2020,
                    Month = 1,
                    Day = 1
                },
                DimDateIdDueNavigation = new DimDate
                {
                    Id = 5,
                    Year = 2021,
                    Month = 1,
                    Day = 1
                },
                DimOrganizations = new[]
                {
                    new DimOrganization
                    {
                        NameFi = "foundation name fi",
                        NameSv = "foundation name sv",
                        NameEn = "foundation name en",
                        OrganizationId = "foundation business id",
                        //DimWebLinks = new []
                        //{
                        //    new DimWebLink
                        //    {
                        //        LinkType = "FoundationURL",
                        //        Url = "http://foundationurl",
                        //    }
                        //}
                    }
                },
                DimReferencedata = new[]
                {
                    new DimReferencedatum
                    {
                        NameFi = "category name fi",
                        NameSv = "category name sv",
                        NameEn = "category name en",
                        CodeValue = "category code"
                    }
                },
                ContactInformation = "contact info",
                ContinuousApplicationPeriod = true

            };
        }

        private static FundingCall GetModel()
        {
            return new FundingCall
            {
                NameFi = "nameFi",
                NameSv = "nameSv",
                NameEn = "nameEn",
                ApplicationTermsFi = "appTermsFi",
                ApplicationTermsSv = "appTermsSv",
                ApplicationTermsEn = "appTermsEn",
                DescriptionFi = "descFi",
                DescriptionSv = "descSv",
                DescriptionEn = "descEn",
                ApplicationURLFi = "http://urlFi",
                ApplicationURLSv = "http://urlSv",
                ApplicationURLEn = "http://urlEn",
                ContactInformation = "contact info",
                CallProgrammeOpenDate = new System.DateTime(2020, 1, 1),
                CallProgrammeDueDate = new System.DateTime(2021, 1, 1),
                ContinuosApplication = true,
                Categories = new[]
                {
                    new Category
                    {
                        CategoryNameFi = "category name fi",
                        CategoryNameSv = "category name sv",
                        CategoryNameEn = "category name en",
                        CategoryCodeValue = "category code"
                    }
                },
                Foundation = new[]
                {
                    new Foundation
                    {
                        FoundationNameFi = "foundation name fi",
                        FoundationNameSv = "foundation name sv",
                        FoundationNameEn = "foundation name en",
                        FoundationBusinessId = "foundation business id",
                        FoundationUrl = "http://foundationurl"
                    }
                }
            };
        }
    }
}
