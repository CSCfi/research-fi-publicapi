/*
 * API version 1.0
 */
using AutoMapper;
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
}