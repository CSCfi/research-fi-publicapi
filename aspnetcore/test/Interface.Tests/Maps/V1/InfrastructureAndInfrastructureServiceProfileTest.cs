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
}