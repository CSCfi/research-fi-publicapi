using AutoMapper;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Maps;

public class FundingCallProfileTest
{
    private readonly IMapper _mapper;

    public FundingCallProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.FundingCallProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}