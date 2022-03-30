using Api.Maps;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void TestFundingDecisionProfile()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
