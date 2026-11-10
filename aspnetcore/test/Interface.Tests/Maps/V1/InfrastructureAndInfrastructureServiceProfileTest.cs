/*
 * API version 1.0
 */
using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Maps.V1;

public class InfrastructureAndInfrastructureServiceProfileTest
{
    private readonly IMapper _mapper;

    /*
     * Test that AutoMapper configuration of InfrastructureProfile and InfrastructureServiceProfile is valid.
     * Test them together because InfrastructureServiceProfile inherits requires models from InfrastructureProfile.
     */
    public InfrastructureAndInfrastructureServiceProfileTest()
    {
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CSC.PublicApi.Interface.Maps.V1.InfrastructureProfile>();
            cfg.AddProfile<CSC.PublicApi.Interface.Maps.V1.InfrastructureServiceProfile>();
        }).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_Infrastructure_PreservesNullCollectionsAndMapsNestedIdentifiers()
    {
        var serviceModel = new CSC.PublicApi.Service.Models.Infrastructure.Infrastructure
        {
            InfraIdentifier = new CSC.PublicApi.Service.Models.Infrastructure.Identifier
            {
                KeyIdentifierURN = "urn:nbn:fi:ttv-synthetic",
                OtherPid = new List<CSC.PublicApi.Service.Models.Infrastructure.PidAttributes>
                {
                    new() { PidType = "doi", PidContent = "10.1234/synthetic" }
                }
            },
            InfraName = new List<CSC.PublicApi.Service.Models.Infrastructure.DescriptiveText>
            {
                new() { TextLanguage = "en", TextContent = "Synthetic infrastructure" }
            },
            InfraRelations = null,
            InfraServices = null
        };

        var result = _mapper.Map<ResearchFi.Infrastructure.Infrastructure>(serviceModel);

        result.InfraIdentifier!.KeyIdentifierURN.Should().Be("urn:nbn:fi:ttv-synthetic");
        result.InfraIdentifier.OtherPid.Should().ContainSingle()
            .Which.PidContent.Should().Be("10.1234/synthetic");
        result.InfraName.Should().ContainSingle()
            .Which.TextContent.Should().Be("Synthetic infrastructure");
        result.InfraRelations.Should().BeNull();
        result.InfraServices.Should().BeNull();
    }

    [Fact]
    public void Map_Infrastructure_MapsEmptyCollectionsWithoutInventingValues()
    {
        var serviceModel = new CSC.PublicApi.Service.Models.Infrastructure.Infrastructure
        {
            InfraName = new List<CSC.PublicApi.Service.Models.Infrastructure.DescriptiveText>(),
            InfraRelations = new List<CSC.PublicApi.Service.Models.Infrastructure.InfrastructureNetwork>(),
            InfraServices = new List<CSC.PublicApi.Service.Models.Infrastructure.InfrastructureService>()
        };

        var result = _mapper.Map<ResearchFi.Infrastructure.Infrastructure>(serviceModel);

        result.InfraIdentifier.Should().BeNull();
        result.InfraName.Should().BeEmpty();
        result.InfraRelations.Should().BeEmpty();
        result.InfraServices.Should().BeEmpty();
    }
}