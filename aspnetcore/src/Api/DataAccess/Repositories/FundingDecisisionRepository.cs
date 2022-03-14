using Api.DatabaseContext;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class FundingDecisionRepository : GenericRepository<DimFundingDecision>, IFundingDecisionRepository
    {
        public FundingDecisionRepository(ApiDbContext context) : base(context)
        {
        }

        public new IAsyncEnumerable<DimFundingDecision> GetAllAsync()
        {
            return dbSet.AsAsyncEnumerable();
        }
    }
}
