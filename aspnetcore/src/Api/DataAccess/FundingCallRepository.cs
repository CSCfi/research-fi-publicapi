using Api.DatabaseContext;
using Api.Models.Entities;

namespace Api.DataAccess
{
    public class FundingCallRepository : GenericRepository<DimCallProgramme>, IFundingCallRepository
    {
        public FundingCallRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
