/*
 * API version 1.0
 */
using AutoMapper;
using CSC.PublicApi.Service.Models;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Maps.V1;

public class FundingCallProfileTest
{
    private readonly IMapper _mapper;

    public FundingCallProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.V1.FundingCallProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_FundingCall_PreservesNestedContractValues()
    {
        var serviceModel = new CSC.PublicApi.Service.Models.FundingCall.FundingCall
        {
            Id = 42,
            NameFi = "Synthetic call",
            Foundations = new()
            {
                new()
                {
                    NameEn = "Synthetic foundation",
                    BusinessId = "1234567-8",
                    FoundationUrl = "https://example.test/foundation"
                }
            },
            Categories = new()
            {
                new ReferenceData { Code = "SYNTHETIC", NameEn = "Synthetic category" }
            },
            ResearchfiUrl = new CSC.PublicApi.Service.Models.ResearchfiUrl(
                "funding-call",
                "synthetic-42")
        };

        var result = _mapper.Map<ResearchFi.FundingCall.FundingCall>(serviceModel);

        result.NameFi.Should().Be("Synthetic call");
        result.Foundations.Should().ContainSingle()
            .Which.BusinessId.Should().Be("1234567-8");
        result.Foundations.Single().FoundationUrl.Should().Be("https://example.test/foundation");
        result.Categories.Should().ContainSingle()
            .Which.Code.Should().Be("SYNTHETIC");
        result.ResearchfiUrl!.En.Should().EndWith("/funding-call/synthetic-42");
    }

    [Fact]
    public void Map_FundingCall_PreservesNullAndEmptyCollections()
    {
        var nullCollections = _mapper.Map<ResearchFi.FundingCall.FundingCall>(
            new CSC.PublicApi.Service.Models.FundingCall.FundingCall
            {
                Foundations = null,
                Categories = null
            });
        var emptyCollections = _mapper.Map<ResearchFi.FundingCall.FundingCall>(
            new CSC.PublicApi.Service.Models.FundingCall.FundingCall
            {
                Foundations = new(),
                Categories = new()
            });

        nullCollections.Foundations.Should().BeNull();
        nullCollections.Categories.Should().BeNull();
        emptyCollections.Foundations.Should().BeEmpty();
        emptyCollections.Categories.Should().BeEmpty();
    }
}