using AutoMapper;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using CSC.PublicApi.DatabaseContext.Entities;
using Xunit;
using FundingCall = CSC.PublicApi.Service.Models.FundingCall.FundingCall;
using CSC.PublicApi.Interface.Maps;
using CSC.PublicApi.Service.Models.FundingCall;

namespace CSC.PublicApi.Tests.Maps;

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
            DimWebLinks = new[]
            {
                new DimWebLink
                {
                    LinkType = "ApplicationURL",
                    LanguageVariant = "fi",
                    Url = "http://urlFi"
                },
                new DimWebLink
                {
                    LinkType = "ApplicationURL",
                    LanguageVariant = "sv",
                    Url = "http://urlSv"
                },
                new DimWebLink
                {
                    LinkType = "ApplicationURL",
                    LanguageVariant = "en",
                    Url = "http://urlEn"
                }
            },
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
                    NameFi = "category name fi",
                    NameSv = "category name sv",
                    NameEn = "category name en",
                    CodeValue = "category code"
                }
            },
            Foundation = new[]
            {
                new Foundation
                {
                    NameFi = "foundation name fi",
                    NameSv = "foundation name sv",
                    NameEn = "foundation name en",
                    BusinessId = "foundation business id",
                    FoundationUrl = "http://foundationurl"
                }
            }
        };
    }
}