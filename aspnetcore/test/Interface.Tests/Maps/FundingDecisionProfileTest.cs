using AutoMapper;
using Xunit;

namespace CSC.PublicApi.Interface.Tests.Maps;

public class FundingDecisionProfileTest
{
    private readonly IMapper _mapper;

    public FundingDecisionProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.FundingDecisionProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}