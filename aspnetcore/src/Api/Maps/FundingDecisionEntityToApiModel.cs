using Api.Models.Entities;
using Api.Models.FundingDecision;

namespace Api.Maps
{
    public class FundingDecisionEntityToApiModel : IMapper<DimFundingDecision, FundingDecision>
    {
        public FundingDecision Map(DimFundingDecision input)
        {
            return new FundingDecision
            {
                NameFi = input.NameFi,
                NameSv = input.NameSv,
                NameEn = input.NameEn,
                DescriptionFi = input.DescriptionFi,
                DescriptionSv = input.DescriptionSv,
                DescriptionEn = input.DescriptionEn
            };
        }
    }
}
