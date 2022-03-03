using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using FluentAssertions;
using Xunit;

namespace Api.Test.Maps
{
    public class FundingCallEntityToApiModelTest
    {
        private FundingCallEntityToApiModel _mapper;

        public FundingCallEntityToApiModelTest()
        {
            _mapper = new FundingCallEntityToApiModel();
        }

        [Fact]
        public void Map_Should_Map()
        {
            // Arrange
            var entity = new DimCallProgramme
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
                DimWebLinks = new[]
                {
                    new DimWebLink
                    {
                        LinkType = "ApplicationUrl",
                        LanguageVariant = "fi",
                        Url = "http://urlFi"
                    },
                    new DimWebLink
                    {
                        LinkType = "ApplicationUrl",
                        LanguageVariant = "sv",
                        Url = "http://urlSv"
                    },
                    new DimWebLink
                    {
                        LinkType = "ApplicationUrl",
                        LanguageVariant = "en",
                        Url = "http://urlEn"
                    }
                },
                DimDateIdOpenNavigation = new DimDate
                {
                    Year = 2020,
                    Month = 1,
                    Day = 1
                },
                DimDateIdDueNavigation = new DimDate
                {
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
                        DimWebLinks = new []
                        {
                            new DimWebLink
                            {
                                LinkType = "FoundationURL",
                                Url = "http://foundationurl",
                            }
                        }
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
                ContactInformation = "contact info"

            };

            // Act
            var model = _mapper.Map(entity);

            // Assert
            model.Should().BeEquivalentTo(new FundingCall
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
            });
        }
    }
}
