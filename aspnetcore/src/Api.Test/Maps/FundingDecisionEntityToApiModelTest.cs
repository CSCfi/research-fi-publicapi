using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingDecision;
using FluentAssertions;
using Xunit;

namespace Api.Test.Maps
{
    public class FundingDecisionEntityToApiModelTest
    {
        private readonly FundingDecisionEntityToApiModel _mapper;

        public FundingDecisionEntityToApiModelTest()
        {
            _mapper = new FundingDecisionEntityToApiModel();
        }

        [Fact]
        public void Should_Map_DimFundingDecision_To_FundingDecision()
        {
            // Arrange
            var entity = GetEntity();

            // Act
            var model = _mapper.Map(entity);

            // Assert
            model.Should().BeEquivalentTo(GetModel());

        }

        private static DimFundingDecision GetEntity()
        {
            return new DimFundingDecision
            {
                NameFi = "name fi",
                NameSv = "name sv",
                NameEn = "name en",
                DescriptionFi = "description fi",
                DescriptionSv = "description sv",
                DescriptionEn = "description en"
            };
        }

        private FundingDecision GetModel()
        {
            return new FundingDecision
            {
                NameFi = "name fi",
                NameSv = "name sv",
                NameEn = "name en",
                DescriptionFi = "description fi",
                DescriptionSv = "description sv",
                DescriptionEn = "description en"
            };
        }

    }
}
